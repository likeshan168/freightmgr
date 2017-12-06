using Serenity;
using System;
using System.Globalization;
using System.Threading;

namespace Allocation.Common.Services
{
    public abstract class PeriodicBackgroundTask : IBackgroundTask
    {
        protected object sync = new object();
        private bool inProgress;
        private DateTime nextRun;

        protected abstract TimeSpan GetInterval();
        protected abstract void InternalRun();

        public virtual void Initialize()
        {
            Reset();
        }

        public virtual void Reset()
        {
            lock (sync)
            {
                nextRun = DateTime.Now.Add(GetInterval());
                Log.Info("Reset: " + this.GetType().Name + " is scheduled for " + nextRun, this.GetType());
            }
        }

        public void Process()
        {
            lock (sync)
            {
                if (inProgress)
                    return;

                if (DateTime.Now < nextRun)
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
            Log.Info("Run: Executing " + this.GetType().Name + " now...", this.GetType());

            var prm = item as RunParameters;

            lock (sync)
            {
                if (inProgress)
                    return;

                inProgress = true;
            }
            try
            {
                try
                {
                    var culture = item as CultureInfo;
                    Thread.CurrentThread.CurrentCulture = prm.CurrentCulture;
                    Thread.CurrentThread.CurrentUICulture = prm.CurrentUICulture;
                    nextRun = DateTime.Now.Add(GetInterval());
                    InternalRun();

                    Log.Info("Run: Done executing " + this.GetType().Name + ".", this.GetType());
                }
                catch (Exception ex)
                {
                    Log.Info("Run: Error while executing " + this.GetType().Name + ".", this.GetType());
                    ex.Log();
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
    }
}