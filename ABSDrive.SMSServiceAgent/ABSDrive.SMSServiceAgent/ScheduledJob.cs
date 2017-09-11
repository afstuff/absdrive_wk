using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Quartz;
using Quartz.Impl;
using ABSDrive.Repositories;

namespace ABSDrive.SMSServiceAgent
{
    class ScheduledJob:IScheduledJob
    {
        public void Run()
        {
            // Get an instance of the Quartz.Net scheduler
            var shd = GetScheduler();

            if (!shd.IsStarted)
                shd.Start();

            // Define the Job to be scheduled
            var job1 = JobBuilder.Create<ReadSMSJob>()
                .WithIdentity("ReadSMSToDB", "IT")
                .RequestRecovery()
                .Build();

            var job2 = JobBuilder.Create<SendNotificationsJob>()
                .WithIdentity("SendNotif", "IT")
                .RequestRecovery()
                .Build();

            // Calculate the start time of trigger as 5 seconds from now
            DateTimeOffset startTimeJob1 = DateBuilder.FutureDate(5, IntervalUnit.Second);
            DateTimeOffset startTimeJob2 = DateBuilder.FutureDate(5, IntervalUnit.Second);

            // Associate a trigger with the Job
            ITrigger trigger1 = TriggerBuilder.Create()
               .WithIdentity("ReadSMSToDB", "IT")
               .StartAt(startTimeJob1)
               .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(120))
               .WithPriority(1)
               .ForJob(job1)
               .Build();

            ITrigger trigger2 = TriggerBuilder.Create()
               .WithIdentity("SendNotif", "IT")
               .StartAt(startTimeJob2)
               .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(120))
               .WithPriority(1)
               .ForJob(job2)
               .Build();

            //check if jobs exists to avoid exception

            if (shd.CheckExists(new JobKey("ReadSMSToDB", "IT")))
            {

                shd.DeleteJob(new JobKey("ReadSMSToDB", "IT"));
            }

            if (shd.CheckExists(new JobKey("SendNotif", "IT")))
            {

                shd.DeleteJob(new JobKey("SendNotif", "IT"));
            }

            // Assign the Job to the scheduler
            var schedule1 = shd.ScheduleJob(job1, trigger1);
            var schedule2 = shd.ScheduleJob(job2, trigger2);

            ActivityLog.ErrorLog("Job 'ReadSMSToDB is scheduled " + schedule1.ToString("r"), "ScheduledJobs_Log_");
            ActivityLog.ErrorLog("Job 'SendNotif is scheduled " + schedule1.ToString("r"), "ScheduledJobs_Log_");

        }

        //get an instance of a scheduler
        private static IScheduler GetScheduler()
        {
            try
            {
                var properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "ServerScheduler";

                // set remoting expoter
                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = string.Format("tcp://{0}:{1}/{2}", "localhost", "555", "QuartzScheduler");

                // Get a reference to the scheduler
                var sf = new StdSchedulerFactory(properties);

                return sf.GetScheduler();

            }

            catch (Exception ex)
            {
                ActivityLog.ErrorLog("Scheduler not available: " + ex.Message, "ScheduledJobs_Log_");
                throw;
            }

        }
    }
}