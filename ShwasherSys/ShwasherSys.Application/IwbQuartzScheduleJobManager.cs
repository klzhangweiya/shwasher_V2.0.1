using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Quartz;
using Abp.Quartz.Configuration;
using Quartz;
using ShwasherSys.Common;

namespace ShwasherSys
{

    public class JobTaskHelp:ITransientDependency
    {
        protected IIwbQuartzScheduleJobManager IwbQuartzScheduleJobManager;

        public JobTaskHelp(IIwbQuartzScheduleJobManager iwbQuartzScheduleJobManager)
        {
            IwbQuartzScheduleJobManager = iwbQuartzScheduleJobManager;
        }
        public void StartJob<T>(string jobKey,string jobKeyGroup, string cronExpress) where T: JobBase
        {
            IwbQuartzScheduleJobManager.DeleteJob(jobKey, jobKeyGroup);
            IwbQuartzScheduleJobManager.ScheduleAsync<T>(job =>
            {
                job.WithIdentity(jobKey, jobKeyGroup);
            }, trigger =>
            {
                trigger.WithIdentity(jobKey, jobKeyGroup)
                    .WithCronSchedule(cronExpress)
                    .ForJob(jobKey, jobKeyGroup)
                    .Build();
            });
        }

        public void StopJob(string jobKey, string jobGroupKey)
        {
            if (IwbQuartzScheduleJobManager.CheckJobIsExist(jobKey, jobGroupKey))
            {
                IwbQuartzScheduleJobManager.DeleteJob(jobKey, jobGroupKey);
            }
        }

    }

    public class IwbQuartzScheduleJobManager : QuartzScheduleJobManager, IIwbQuartzScheduleJobManager
    {
        private readonly IBackgroundJobConfiguration _backgroundJobConfiguration;
        private readonly IAbpQuartzConfiguration _quartzConfiguration;

        public IwbQuartzScheduleJobManager(IAbpQuartzConfiguration quartzConfiguration, IBackgroundJobConfiguration backgroundJobConfiguration):base(quartzConfiguration, backgroundJobConfiguration)
        {
            this._quartzConfiguration = quartzConfiguration;
            this._backgroundJobConfiguration = backgroundJobConfiguration;
        }

        public bool CheckJobIsExist(JobKey key)
        {
            return _quartzConfiguration.Scheduler.CheckExists(key).Result;
        }

        public bool CheckJobIsExist(string jobName, string jobGroupName="")
        {
            if (jobGroupName.IsNullOrEmpty())
            {
                jobGroupName = jobName + "-G";
            }
            JobKey key = new JobKey(jobName, jobGroupName);
            return CheckJobIsExist(key);
        }

        public async Task DeleteJob(string jobKey, string jobGroupKey)
        {
            if (jobGroupKey.IsNullOrEmpty())
            {
                jobGroupKey = jobKey + "-G";
            }
            JobKey key = new JobKey(jobKey, jobGroupKey);
            await _quartzConfiguration.Scheduler.DeleteJob(key);
        }
     
    }
    public interface IIwbQuartzScheduleJobManager : IQuartzScheduleJobManager
    {
        /// <summary>
        /// 检查Job是否已存在;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool CheckJobIsExist(JobKey key);
        bool CheckJobIsExist(string jobName, string jobGroupName="");

        Task DeleteJob(string jobKey, string jobGroupKey = "");

    }
}
