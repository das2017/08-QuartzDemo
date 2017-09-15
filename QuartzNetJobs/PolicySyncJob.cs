using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuartzNetJobs
{
    /// <summary>
    /// 政策同步任务
    /// </summary>
    public class PolicySyncJob : IJob
    {
        ILog logger = LogManager.GetLogger(typeof(PolicySyncJob));

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine(string.Format("[{0}] 政策同步处理中...", DateTime.Now.ToString("HH:mm:ss")));
            logger.Info(string.Format("[{0}] 政策同步处理中...", DateTime.Now.ToString("HH:mm:ss")));
            //政策同步业务逻辑处理
            System.Threading.Thread.Sleep(3000);
        }
    }
}
