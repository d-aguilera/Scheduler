using Scheduler.DataAccess;
using Scheduler.DataContracts;
using Scheduler.SchedulerService.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace Scheduler.CronService
{
    public partial class Service : ServiceBase
    {
        private Timer _timer = new Timer();
        private Timer _firstCheckTimer = new Timer();
        private int[] _scheduleEntryIds = new int[0];

        public Service()
        {
            InitializeComponent();
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Interval = 60000.0;

            var lastCheck = DateTime.UtcNow;

            if (_scheduleEntryIds.Length > 0)
            {
                var channel = SchedulerServiceClientFactory.CreateChannel();
                channel.ExecuteMany(_scheduleEntryIds);
            }

            var nextCheck = NextExactMinute(lastCheck);
            Reload(nextCheck);
        }

        void Reload(DateTime nextCheck)
        {
            var newScheduleEntryIds = new List<int>();

            IEnumerable<ScheduleEntry> scheduleEntries;
            using (var context = new WebContext())
            {
                scheduleEntries = context.ScheduleEntries
                    .Where(se => se.Enabled)
                    .ToList();
            }

            foreach (var scheduleEntry in scheduleEntries)
            {
                var cronParts = scheduleEntry.CronExpression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (cronParts.Length < 5)
                    continue;

                // Check minute
                if (!CronPartMatches(cronParts[0], nextCheck.Minute))
                    continue;

                // Check hour
                if (!CronPartMatches(cronParts[1], nextCheck.Hour))
                    continue;

                // Check day of month
                if (!CronPartMatches(cronParts[2], nextCheck.Day))
                    continue;

                // Check month
                if (!CronPartMatches(cronParts[3], nextCheck.Month))
                    continue;

                // Check day of week
                if (!CronPartMatches(cronParts[4], (int)nextCheck.DayOfWeek))
                    continue;

                newScheduleEntryIds.Add(scheduleEntry.Id);
            }

            _scheduleEntryIds = newScheduleEntryIds.ToArray();
        }

        protected override void OnStart(string[] args)
        {
            _scheduleEntryIds = new int[0];

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += Timer_Elapsed;

            var now = DateTime.UtcNow;
            var nextCheck = NextExactMinute(now);
            Reload(nextCheck);

            now = DateTime.UtcNow;

            var millisToNextCheck = (nextCheck - now).TotalMilliseconds;
            if (millisToNextCheck > 0)
            {
                _timer.Interval = millisToNextCheck;
            }
            else
            {
                Timer_Elapsed(null, null);

                nextCheck = NextExactMinute(now);
                Reload(nextCheck);

                millisToNextCheck = (nextCheck - now).TotalMilliseconds;
                _timer.Interval = millisToNextCheck;
            }

            _timer.Start();
        }

        protected override void OnCustomCommand(int command)
        {
            const int CronReload = 1;

            base.OnCustomCommand(command);

            switch (command)
            {
                case CronReload:
                    var nextCheck = NextExactMinute(DateTime.UtcNow);
                    Reload(nextCheck);
                    break;
            }
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        static DateTime NextExactMinute(DateTime from)
        {
            return new DateTime(from.Year, from.Month, from.Day, from.Hour, from.Minute, 0, 0, DateTimeKind.Utc).AddMinutes(1.0);
        }

        static bool CronPartMatches(string cronPart, int target)
        {
            return cronPart == "*" || int.Parse(cronPart) == target;
        }
    }
}
