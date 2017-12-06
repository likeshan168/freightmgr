using Serenity;
using System;
using System.Globalization;
using System.Threading;

namespace Allocation.Common.Services
{
    public abstract class DailyBackgroundTask : IBackgroundTask
    {
        protected object sync = new object();
        private bool inProgress;
        private DateTime nextRun;
        private int retryCount;

        public virtual void Initialize()
        {
            Reset();
        }

        public virtual void Reset()
        {
            lock (sync)
            {
                retryCount = 0;
                if (DateTime.Now < DateTime.Today.Add(GetRunAtTime()))
                    nextRun = DateTime.Today;
                else
                    nextRun = DateTime.Today.AddDays(1);

                Log.Info("Reset: " + this.GetType().Name + " is scheduled for " + nextRun.Add(GetRunAtTime()), this.GetType());
            }
        }

        public void Process()
        {
            lock (sync)
            {
                if (inProgress)
                    return;

                var actualNextRun = retryCount == 0 ? nextRun.Add(GetRunAtTime()) : nextRun;

                if (DateTime.Now < actualNextRun)
                    return;
            }

            ThreadPool.QueueUserWorkItem(Run, new RunParameters
            {
                CurrentCulture = CultureInfo.CurrentCulture,
                CurrentUICulture = CultureInfo.CurrentUICulture
            });
        }

        private class RunParameters
        {
            public CultureInfo CurrentCulture { get; set; }
            public CultureInfo CurrentUICulture { get; set; }
        }

        private void Run(object item)
        {
            var prm = item as RunParameters;

            lock (sync)
            {
                if (inProgress)
                    return;

                inProgress = true;
            }
            try
            {
                Log.Info("Run: Executing " + this.GetType().Name + " now...", this.GetType());

                try
                {
                    Thread.CurrentThread.CurrentCulture = prm.CurrentCulture;
                    Thread.CurrentThread.CurrentUICulture = prm.CurrentUICulture;

                    InternalRun();
                    nextRun = DateTime.Today.AddDays(1);
                    retryCount = 0;

                    Log.Info("Run: Done executing " + this.GetType().Name + ".", this.GetType());
                    Log.Info("Run: " + this.GetType().Name + " is rescheduled for " + nextRun.Add(GetRunAtTime()), this.GetType());
                }
                catch (Exception ex)
                {
                    Log.Error("Run: Error while executing " + this.GetType().Name + ".", this.GetType());
                    ex.Log();

                    lock (sync)
                    {
                        if (retryCount < GetMaxRetry())
                        {
                            retryCount++;
                            nextRun = DateTime.Now.AddMinutes(GetRetryInterval());
                            Log.Info("Run: " + this.GetType().Name + " is rescheduled for " + nextRun.Add(GetRunAtTime()), this.GetType());
                        }
                        else
                        {
                            nextRun = DateTime.Today.AddDays(1);
                            Log.Info("Run: " + this.GetType().Name + " is rescheduled for " + nextRun, this.GetType());
                            retryCount = 0;
                        }
                    }
                }
            }
            finally
            {
                lock (sync)
                {
                    inProgress = false;
                }
            }
        }

        protected virtual int GetMaxRetry()
        {
            return 5;
        }

        protected virtual int GetRetryInterval()
        {
            return 10;
        }

        protected abstract void InternalRun();
        protected abstract TimeSpan GetRunAtTime();
    }
}