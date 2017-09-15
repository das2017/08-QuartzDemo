using log4net;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace QuartzNetDemo.WindowsService
{
    public partial class JobService : ServiceBase
    {
        ILog logger = LogManager.GetLogger(typeof(JobService));
        /// <summary>
        /// 当前调度服务的调度器
        /// </summary>
        public IScheduler CurrentSched { get; private set; }

        public JobService()
        {
            InitializeComponent();

            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
            CurrentSched = schedulerFactory.GetScheduler();
        }

        protected override void OnStart(string[] args)
        {
            CurrentSched.Start();
            logger.Info("调度服务成功启动！");
        }

        protected override void OnStop()
        {
            CurrentSched.Shutdown();
            logger.Info("调度服务成功终止！");
        }
    }
}
