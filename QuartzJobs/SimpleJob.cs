using log4net;
using Quartz;
using System;
using System.Threading;

namespace QuartzJobs
{
    /// <summary>
    /// 简单任务
    /// </summary>
    public class SimpleJob : IJob
    {
        ILog logger = LogManager.GetLogger(typeof(SimpleJob));

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine(string.Format("[{0}] 简单的任务处理中...", DateTime.Now.ToString("HH:mm:ss")));
            logger.Info(string.Format("[{0}] 简单的任务处理中...", DateTime.Now.ToString("HH:mm:ss")));
            //业务逻辑处理
            Thread.Sleep(2000);
        }
    }
}
