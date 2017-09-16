using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using QuartzJobs;
using System;
using System.Threading;

namespace QuartzDemo
{
    class Program
    {
        public static IScheduler CurrentSched { get; private set; }

        static void Main(string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(Program));

            CurrentSched = StdSchedulerFactory.GetDefaultScheduler();   //从工厂中获取一个调度器实例化

            JobWithSimpleTriggerDemo();

            //JobWithCronTriggerDemo();

            //JobWithManyTriggerDemo();

            //CalendarDemo();

            log.Info("------- 开始计划 -----------------");
            CurrentSched.Start();  //开启调度器            

            log.Info("------- 等待20秒-----------------");
            Thread.Sleep(TimeSpan.FromSeconds(20));            
            
            CurrentSched.Shutdown(true);
            log.Info("------- 关闭计划 ---------------------");
        }

        /// <summary>
        /// 演示简单触发器的使用
        /// </summary>
        private static void JobWithSimpleTriggerDemo()
        {
            IJobDetail simpleJob = JobBuilder.Create<SimpleJob>().WithIdentity("任务名称1", "任务组名").Build();    //创建一个任务
            ITrigger simpleTrigger = TriggerBuilder.Create().WithIdentity("触发器名称1", "触发器组名").StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()).Build();   //创建一个简单触发器，每隔5秒执行一次

            CurrentSched.ScheduleJob(simpleJob, simpleTrigger);    //把任务、简单触发器加入调度器
        }

        /// <summary>
        /// 演示Cron触发器的使用
        /// </summary>
        private static void JobWithCronTriggerDemo()
        {
            IJobDetail simpleJob = JobBuilder.Create<SimpleJob>().WithIdentity("任务名称2", "任务组名").Build();    //创建一个任务
            ITrigger cronTrigger = TriggerBuilder.Create().WithIdentity("触发器名称2", "触发器组名").StartNow()
                .WithCronSchedule("2/10 * * ? * *").Build();   //创建一个Cron触发器，每隔10秒执行一次，触发的时刻：02s、12s、22s、32s、42s、52s

            CurrentSched.ScheduleJob(simpleJob, cronTrigger);    //把任务、Cron触发器加入调度器
        }

        /// <summary>
        /// 演示一个任务多触发器的使用
        /// </summary>
        private static void JobWithManyTriggerDemo()
        {
            IJobDetail simpleJob = JobBuilder.Create<SimpleJob>().WithIdentity("任务名称", "任务组名").Build();    //创建一个simpleJob任务
            ITrigger simpleTrigger = TriggerBuilder.Create().WithIdentity("触发器名称3", "触发器组名").StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()).Build();   //创建一个简单触发器，每隔5秒执行一次

            CurrentSched.ScheduleJob(simpleJob, simpleTrigger);    //把simpleJob任务、简单触发器加入调度器

            ITrigger cronTrigger = TriggerBuilder.Create().WithIdentity("触发器名称4", "触发器组名").StartNow()
                .WithCronSchedule("/10 * * ? * *").ForJob(simpleJob).Build();   //创建一个为任务“simpleJob”服务的Cron触发器，每隔10秒执行一次

            CurrentSched.ScheduleJob(cronTrigger);   //把Cron触发器加入调度器
        }

        /// <summary>
        /// 演示日历功能的使用
        /// </summary>
        private static void CalendarDemo()
        {
            string holiday = "我的假期";

            DateTime newYear = new DateTime(DateTime.UtcNow.Year, 1, 1);

            HolidayCalendar cal = new HolidayCalendar();
            cal.AddExcludedDate(newYear);
            CurrentSched.AddCalendar(holiday, cal, false, false);

            IJobDetail calJob = JobBuilder.Create<SimpleJob>().WithIdentity("日历任务名称", "日历任务组名").Build();    //创建一个日历任务

            ITrigger calTrigger = TriggerBuilder.Create().WithIdentity("触发器名称", "触发器组名").StartNow()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(14, 13)).ModifiedByCalendar(holiday).Build();  //每天上午9:30分执行，但新年的第1天不执行

            CurrentSched.ScheduleJob(calJob, calTrigger);
        }
    }
}
