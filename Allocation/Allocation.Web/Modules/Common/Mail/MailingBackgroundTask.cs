
namespace Allocation.Common.Services
{
    using Entities;
    using Serenity;
    using Serenity.Data;
    using System;

    public class MailingBackgroundTask : PeriodicBackgroundTask
    {
        protected override TimeSpan GetInterval()
        {
            var config = Config.Get<MailingServiceSettings>();
            return TimeSpan.FromSeconds(config.Interval);
        }

        protected override void InternalRun()
        {
            var env = Config.Get<EnvironmentSettings>();
            var config = Config.Get<MailingServiceSettings>();

            if (!config.Enabled)
                return;

            using (var connection = SqlConnections.NewFor<MailRow>())
            {
                var m = MailRow.Fields;

                var mailList = connection.List<MailRow>(q => q
                    .Take(config.BatchSize)
                    .Select(m.MailId)
                    .Select(m.LockExpiration)
                    .Where(
                        m.Status == (int)MailStatus.InQueue &
                        m.RetryCount < config.RetryLimit &
                        m.LockExpiration < DateTime.Now)
                    .OrderBy(m.Priority)
                    .OrderBy(m.RetryCount)
                    .OrderBy(m.MailId));

                var lockDuration = TimeSpan.FromSeconds(config.LockDuration);

                foreach (var mail in mailList)
                {
                    try
                    {
                        using (var uow = new UnitOfWork(connection))
                        {
                            if (SqlApplicationLock.Get(uow, "BackgroundTask_MailingSend_" + mail.MailId.Value) < 0)
                                continue;

                            if (new SqlUpdate(m.TableName)
                                .Set(m.LockExpiration, DateTime.Now.Add(lockDuration))
                                .Where(
                                    m.MailId == mail.MailId.Value &
                                    m.LockExpiration == mail.LockExpiration.Value)
                                .Execute(connection, ExpectedRows.ZeroOrOne) == 1)
                            {
                                uow.Commit();
                            }
                        }

                        new MailingService().SendById(connection, mail.MailId.Value);
                    }
                    catch (Exception ex)
                    {
                        ex.Log();
                    }
                }
            }
        }
    }
}