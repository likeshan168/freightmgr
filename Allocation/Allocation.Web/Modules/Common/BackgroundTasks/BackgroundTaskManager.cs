using Serenity;
using Serenity.ComponentModel;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Allocation.Common.Services
{
    public static class BackgroundTaskManager
    {
        private static Timer timer;
        private static Double timerInterval = TimeSpan.FromSeconds(10).TotalMilliseconds;
        private static List<IBackgroundTask> tasks;
        private static bool isDisabled;

        static BackgroundTaskManager()
        {
            timer = new Timer();
            timer.Interval = timerInterval;
            timer.Elapsed += Timer_Elapsed;
            tasks = new List<IBackgroundTask>();

            isDisabled = !Config.Get<Settings>().Enabled;
        }

        public static void Register(IBackgroundTask task)
        {
            tasks.Add(task);
        }

        public static void Initialize()
        {
            if (isDisabled)
                return;

            foreach (var service in tasks)
            {
                try
                {
                    service.Initialize();
                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            }

            timer.Start();
        }

        public static void Reset()
        {
            if (isDisabled)
                return;

            foreach (var service in tasks)
            {
                try
                {
                    service.Reset();
                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            }
        }

        private static void Process()
        {
            if (isDisabled)
                return;

            foreach (var service in tasks)
            {
                try
                {
                    service.Process();
                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            }
        }

        private static void Timer_Elapsed(object sender, EventArgs e)
        {
            Process();
        }

        [SettingScope("Application"), SettingKey("BackgroundTasks")]
        public class Settings
        {
            public bool Enabled { get; set; }
        }
    }
}