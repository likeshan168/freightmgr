
namespace Allocation.Common.Services
{
    using Serenity.ComponentModel;

    [SettingScope("Application"), SettingKey("MailingService")]
    public class MailingServiceSettings
    {
        public bool Enabled { get; set; }
        public bool AutoUse { get; set; }
        public int LockDuration { get; set; }
        public int Interval { get; set; }
        public int BatchSize { get; set; }
        public int RetryLimit { get; set; }

        public MailingServiceSettings()
        {
            Enabled = true;
            AutoUse = true;
            Interval = 30;
            BatchSize = 10;
            LockDuration = 300;
            RetryLimit = 5;
        }
    }
}