
namespace Allocation.Common.Services
{
    using Entities;
    using Serenity;
    using Serenity.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Web.Hosting;

    public class MailingService
    {
        private static readonly char[] mailSplitter = { ',', ';' };

        private IEnumerable<string> SplitEmails(string s)
        {
            if (s == null)
                return new string[0];

            return s.Split(mailSplitter, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private MailMessage BuildMessage(MailRow mail)
        {
            var message = new MailMessage();
            message.Body = mail.Body;
            message.Subject = mail.Subject;
            if (!string.IsNullOrEmpty(mail.MailFrom))
                message.From = new MailAddress(mail.MailFrom);

            message.IsBodyHtml = true;

            foreach (var x in SplitEmails(mail.ReplyTo))
                message.ReplyToList.Add(x);

            foreach (var x in SplitEmails(mail.MailTo))
                message.To.Add(x);

            foreach (var x in SplitEmails(mail.Cc))
                message.CC.Add(x);

            foreach (var x in SplitEmails(mail.Bcc))
                message.Bcc.Add(x);

            return message;
        }

        public void Enqueue(IDbConnection connection, MailMessage message, Guid? uid = null)
        {
            var mail = new MailRow();
            mail.Uid = uid ?? Guid.NewGuid();
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.Priority = message.Priority == MailPriority.Normal ? MailQueuePriority.Medium
                : (message.Priority == MailPriority.Low ? MailQueuePriority.Low : MailQueuePriority.High);
            mail.Status = MailStatus.InQueue;
            mail.LockExpiration = DateTime.Now.AddDays(-1);
            mail.InsertDate = DateTime.Now;
            mail.InsertUserId = Authorization.UserId == null ? (int?)null : Convert.ToInt32(Authorization.UserId);
            mail.RetryCount = 0;
            mail.MailFrom = message.From != null ? message.From.ToString() : null;
            if (message.To.Count > 0)
                mail.MailTo = string.Join(";", message.To.Select(x => x.ToString()));
            if (message.CC.Count > 0)
                mail.Cc = string.Join(";", message.CC.Select(x => x.ToString()));
            if (message.Bcc.Count > 0)
                mail.Bcc = string.Join(";", message.Bcc.Select(x => x.ToString()));
            if (message.ReplyToList.Count > 0)
                mail.ReplyTo = string.Join(";", message.ReplyToList.Select(x => x.ToString()));
            connection.Insert(mail);
        }

        public bool SendById(IDbConnection connection, long mailId)
        {
            var mail = connection.ById<MailRow>(mailId);
            var settings = Config.Get<MailingServiceSettings>();
            var status = MailStatus.Sent;
            var retryCount = (mail.RetryCount ?? 0) + 1;

            string errorMessage = null;
            try
            {
                var config = Config.Get<MailingServiceSettings>();
                var env = Config.Get<EnvironmentSettings>();
                var message = BuildMessage(mail);
                var client = new SmtpClient();

                if (client.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory &&
                    string.IsNullOrEmpty(client.PickupDirectoryLocation))
                {
                    var pickupPath = HostingEnvironment.MapPath("~/App_Data");
                    pickupPath = Path.Combine(pickupPath, "Mail");
                    Directory.CreateDirectory(pickupPath);
                    client.PickupDirectoryLocation = pickupPath;
                }

                client.Send(message);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                status = retryCount < settings.RetryLimit ?
                    MailStatus.InQueue : MailStatus.Failed;
            }

            connection.UpdateById(new MailRow
            {
                MailId = mail.MailId,
                Status = status,
                SentDate = DateTime.Now,
                ErrorMessage = errorMessage,
                RetryCount = retryCount
            });

            return errorMessage != null;
        }
    }
}