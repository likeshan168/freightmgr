using System.Web.Hosting;
using System.IO;
using System.Net.Mail;
using Allocation.Common.Entities;
using System;
using Serenity;
using Allocation.Common.Services;
using Serenity.Data;

namespace Allocation.Common
{
    public class EmailHelper
    {
        public static void Send(string subject, string body, string address, string displayName = "")
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(address, ""));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            Send(message);
        }

        public static void Send(MailMessage message)
        {
            var settings = Config.Get<MailingServiceSettings>();
            var client = new SmtpClient();

            if (client.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory &&
                string.IsNullOrEmpty(client.PickupDirectoryLocation))
            {
                var pickupPath = HostingEnvironment.MapPath("~/App_Data");
                pickupPath = Path.Combine(pickupPath, "Mail");
                Directory.CreateDirectory(pickupPath);
                client.PickupDirectoryLocation = pickupPath;
            }

            if (settings.AutoUse)
            {
                using (var connection = SqlConnections.NewFor<MailRow>())
                    new MailingService().Enqueue(connection, message);
            }
            else
                client.Send(message);
        }
    }
}