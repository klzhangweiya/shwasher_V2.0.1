using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace ShwasherSys.QuartzHelpers
{

    public class QuartzAsyncHelper
    {
        public static QuartzAsyncHelper Instance { get; set; } = new QuartzAsyncHelper();

        #region Field Properties

        private readonly ILog _log = LogManager.GetLogger(typeof(QuartzAsyncHelper));
        public ISchedulerFactory SchedulerFactory { get; set; }
        public IScheduler Sched { get; set; }
        public string JobName { get; set; }
        public string JobGroupName { get; set; }
        public string TriggerName { get; set; }
        public string TriggerGroupName { get; set; }
        public IJobDetail JobDetail { get; set; }
        public ITrigger Trigger { get; set; }
        DateTimeOffset? TimeOffset { get; set; }
        public string ErrorMsg { get; set; }

        //public ICronTrigger CronTrigger { get; set; }
        //public ISimpleTrigger SimpleTrigger { get; set; }

        #endregion

        #region Initialize

        private async Task<IScheduler> GetScheduler()
        {
            IScheduler scheduler = null;
            try
            {
                scheduler = await SchedulerFactory.GetScheduler();
            }
            catch (SchedulerException e)
            {
                LogManager.GetLogger(typeof(QuartzAsyncHelper)).Info(e.Message);
            }
            return scheduler;
        }

        public QuartzAsyncHelper()
        {
            SchedulerFactory = new StdSchedulerFactory();
            Sched = GetScheduler().Result;
        }

        public QuartzAsyncHelper(string jobName, string jobGroupName) : this()
        {
            TriggerName = JobName = jobName;
            JobGroupName = JobGroupName = jobGroupName;
        }

        public QuartzAsyncHelper(string jobName, string jobGroupName, string triggerName, string triggerGroupName) : this(jobName, jobGroupName)
        {
            TriggerName = triggerName;
            JobGroupName = triggerGroupName;
        }

        public QuartzAsyncHelper(string jobName, string jobGroupName, IJobDetail jobDetail, ITrigger trigger) : this(jobName, jobGroupName)
        {
            JobDetail = jobDetail;
            Trigger = trigger;
        }

        public QuartzAsyncHelper(IJobDetail jobDetail, ITrigger trigger)
        {
            SchedulerFactory = new StdSchedulerFactory();
            Sched = SchedulerFactory.GetScheduler().Result;
            JobDetail = jobDetail;
            Trigger = trigger;
        }
        #endregion

        #region Create JobDetail

        /// <summary>
        /// 向Scheduler(调度程序)中添加Job
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateJob()
        {
            if (JobDetail != null && Trigger != null)
            {
                if (Sched.IsShutdown)
                {
                    SchedulerFactory = new StdSchedulerFactory();
                    Sched = await SchedulerFactory.GetScheduler();
                }
                if (await CheckJobIsExist(JobDetail))
                    ErrorMsg = "JobtType:" + JobDetail.Key + " 已存在！";
                TimeOffset = await Sched.ScheduleJob(JobDetail, Trigger);
                _log.Info("[" + JobDetail.Key + "] will run at: " + TimeOffset + "---->");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 向Scheduler(调度程序)中添加Job
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public async Task<bool> CreateJob(IJobDetail jobDetail, ITrigger trigger)
        {
            ErrorMsg = "";
            if (jobDetail != null && trigger != null)
            {
                if (Sched.IsShutdown)
                {
                    SchedulerFactory = new StdSchedulerFactory();
                    Sched = await SchedulerFactory.GetScheduler();
                }
                if (await CheckJobIsExist(jobDetail))
                    ErrorMsg = "JobtType:[" + jobDetail.Key + "] Is Exist！";
                else
                {
                    try
                    {
                        TimeOffset = await Sched.ScheduleJob(jobDetail, trigger);
                        _log.Info("JobtType:[" + jobDetail.Key + "] will run at: " + TimeOffset + "---->");

                        return true;
                    }
                    catch (Exception e)
                    {
                        _log.Error(e.Message);
                        ErrorMsg = "System Error!";
                    }

                }
            }
            else
            {
                ErrorMsg = "JobDetail/Trigger is null";
            }
            return false;
        }

        /// <summary>
        /// 向Scheduler(调度程序)中添加Job
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="trigger"></param>
        /// <param name="jobName"></param>
        /// <param name="jobGroupName"></param>
        /// <returns></returns>
        public async Task<bool> CreateJob(Type jobType, ITrigger trigger, string jobName = null, string jobGroupName = null)
        {
            IJobDetail jobDetail = GetJobDetail(jobType, jobName, jobGroupName);
            if (jobDetail == null)
                return false;
            return await CreateJob(jobDetail, trigger);
        }

        /// <summary>
        /// 向Scheduler(调度程序)中添加Job
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="triggerStr">
        /// 触发器参数：【"开始时间"|"开始时间单位(参照IntervalUnit)"|"间隔时间"|"间隔时间单位(参照IntervalUnit)"|"重复次数(0代表无限循环)"】
        /// 【"cronExpStr"】(特定时间执行，填*代表每年每月...，填具体值代表特定值(多个值,间隔、范围值用-,a/b 指 从a开始，每隔b 触发一次))
        /// </param>
        /// <param name="triggerType">触发器类型</param>
        /// <param name="jobName"></param>
        /// <param name="jobGroupName"></param>
        /// <returns></returns>
        public async Task<bool> CreateJob(Type jobType, string triggerStr, TriggerType triggerType = TriggerType.SimpleTrigger, string jobName = null, string jobGroupName = null)
        {
            IJobDetail jobDetail = GetJobDetail(jobType, jobName, jobGroupName);
            if (jobDetail == null)
                return false;
            ITrigger trigger;
            switch (triggerType)
            {
                #region SimpleTrigger
                case TriggerType.SimpleTrigger:
                    trigger = await GetSimpleTrigger(triggerStr);
                    if (trigger != null)
                        return await CreateJob(jobDetail, trigger);
                    break;
                #endregion

                #region CronTrigger
                case TriggerType.CronTrigger:
                    string cronExpStr = triggerStr;
                    //if (!string.IsNullOrEmpty(triggerArr[0]))
                    //	cronExpStr = GetCronExpString(triggerArr[0]);
                    //else if (triggerArr.Length == 2)
                    //	cronExpStr = triggerArr[1];
                    trigger = await GetCronTrigger(cronExpStr);

                    if (trigger != null)
                        return await CreateJob(jobDetail, trigger);
                    break;
                    #endregion
            }

            return false;
        }

        /// <summary>
        /// 向Scheduler(调度程序)中添加Job
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="cronExpStr">CronSchedule 触发器表达式</param>
        /// <param name="jobName"></param>
        /// <param name="jobGroupName"></param>
        /// <returns></returns>
        public async Task<bool> CreateJob(Type jobType, string cronExpStr, string jobName = null, string jobGroupName = null)
        {
            IJobDetail jobDetail = GetJobDetail(jobType, jobName, jobGroupName);
            if (jobDetail == null)
                return false;
            ITrigger trigger = await GetCronTrigger(cronExpStr);
            if (trigger != null)
                return await CreateJob(jobDetail, trigger);
            return false;
        }

        /// <summary>
        /// 向Scheduler(调度程序)中添加多个job任务
        /// </summary>
        /// <param name="triggersAndJobs"></param>
        /// <param name="replace"></param>
        public async Task CreateJobs(IReadOnlyDictionary<IJobDetail, IReadOnlyCollection<ITrigger>> triggersAndJobs, bool replace)
        {
            await Sched.ScheduleJobs(triggersAndJobs, replace);
        }

        /// <summary>
        /// 添加 JobDetail
        /// </summary>
        /// <param name="jobdetail"></param>
        /// <param name="replace">是否覆盖</param>
        public async Task AddJob(IJobDetail jobdetail, bool replace)
        {
            await Sched.AddJob(jobdetail, replace);
        }

        #endregion

        #region Get JobDetail

        /// <summary>
        /// 获取 JobDetail
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IJobDetail> GetJobDetail(JobKey key)
        {
            return await CheckJobIsExist(key) ? await Sched.GetJobDetail(key) : null;
        }

        /// <summary>
        /// 获取 JobDetail
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="jobGroupName"></param>
        /// <returns></returns>
        public async Task<IJobDetail> GetJobDetail(string jobName, string jobGroupName)
        {
            JobKey key = GetJobKey(jobName, jobGroupName);
            return await GetJobDetail(key);
        }

        /// <summary>
        /// 构造 JobDetail
        /// </summary>
        /// <param name="jobType"></param>
        /// <param name="jobName"></param>
        /// <param name="jobGroupName"></param>
        /// <returns></returns>
        public IJobDetail GetJobDetail(Type jobType, string jobName = null, string jobGroupName = null)
        {
            jobName = jobName ?? JobName;
            jobGroupName = jobGroupName ?? JobGroupName;
            if (string.IsNullOrEmpty(jobName) || string.IsNullOrEmpty(jobGroupName))
            {
                ErrorMsg = "jobName/jobGroupName is empty!";
                return null;
            }
            IJobDetail jobDetail = null;
            try
            {
                jobDetail = new JobDetailImpl(jobName, jobGroupName, jobType);
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                ErrorMsg = e.Message;

            }
            return jobDetail;
        }

        /// <summary>
        /// 获取job 组名
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<string>> GetJobGroupNames()
        {
            return await Sched.GetJobGroupNames();
        }

        /// <summary>
        /// 给JobDetail设置参数
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <param name="parmDic">参数的值</param>
        /// <param name="parmKey">参数的键</param>
        public void SetJobDetailParm(IJobDetail jobDetail, Dictionary<string, object> parmDic, string parmKey = "jobParm")
        {
            var map = jobDetail.JobDataMap.Get(parmKey);
            if (map != null)
            {
                jobDetail.JobDataMap[parmKey] = parmDic;
            }
            else
            {
                jobDetail.JobDataMap.Put(parmKey, parmDic);
            }
        }

        #endregion

        #region Delete JobDetail

        public async Task<bool> DeleteJob(string jobName = null, string jobGroupName = null)
        {
            jobName = jobName ?? JobName;
            jobGroupName = jobGroupName ?? JobGroupName;
            return await DeleteJob(GetJobKey(jobName, jobGroupName));
        }

        public async Task<bool> DeleteJob(IJobDetail jobDetail)
        {
            return jobDetail != null && await Sched.DeleteJob(jobDetail.Key);
        }

        public async Task<bool> DeleteJob(JobKey key)
        {
            return await Sched.DeleteJob(key);
        }

        public async Task<bool> DeleteJobs(string jobGroupName = null)
        {
            jobGroupName = jobGroupName ?? JobGroupName;
            var keys = await GetJobKeys(jobGroupName);
            return await Sched.DeleteJobs(keys);
        }

        public async Task<bool> DeleteJobs(List<JobKey> keys)
        {
            return await Sched.DeleteJobs(keys);
        }

        /*
		 * 首先从Scheduler.scheduleJob(JobDetail jobDetail, Trigger trigger) 调度job，实际上就是将job存储到RAM中的jobsByGroup,jobsByKey对应的Map中， 将trigger存储到triggers（List），triggersByKey，triggersByGroup对应的Map中，及timeTriggers的Treeset中。
		 * Scheduler.unscheduleJob(TriggerKey triggerKey) 就是将triggerKey从triggersByKey，triggersByGroup，triggers，timeTriggers中移除；
		 * Scheduler.deleteJob(JobKey jobKey)除了从容器triggers中的TriggerWrapper的JobKey为jobKey的List<TriggerWrapper>，并unscheduleJob(TriggerKey triggerKey)列表 List<TriggerWrapper>中的所有TriggerWrapper，同时从jobsByKey，jobsByGroup的移除对应jobKey的相关信息 
		 */

        /// <summary>
        /// 停止调度Job任务
        /// </summary>
        /// <param name="triggerkey"></param>
        /// <returns></returns>
        /// <para>If the related job does not have any other triggers, and the job is
        /// not durable, then the job will also be deleted.</para>
        public async Task<bool> UnscheduleJob(TriggerKey triggerkey)
        {
            return await Sched.UnscheduleJob(triggerkey);
        }

        /// <summary>
        /// 停止多个调度Job任务
        /// </summary>
        /// <param name="triggerkeys"></param>
        /// <returns></returns>
        public async Task<bool> UnscheduleJobs(List<TriggerKey> triggerkeys)
        {
            return await Sched.UnscheduleJobs(triggerkeys);
        }

        /// <summary>
        /// 重新恢复触发器相关的job任务  
        /// </summary>
        /// <param name="triggerkey"></param>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public async Task<DateTimeOffset?> RescheduleJob(TriggerKey triggerkey, ITrigger trigger)
        {
            return await Sched.RescheduleJob(triggerkey, trigger);
        }
        #endregion

        #region Get Trigger

        public async Task<ITrigger> GetTrigger(TriggerKey key)
        {
            return await CheckTriggerIsExist(key) ? await Sched.GetTrigger(key) : null;
        }

        public async Task<ITrigger> GetTrigger(string triggerName, string triggerGroupName)
        {
            var key = GetTriggerKey(triggerName, triggerGroupName);
            return await GetTrigger(key);
        }

        /// <summary>
        /// 获取 指定 Job 里的所有 Trigger
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<ITrigger>> GetTriggersByJob(JobKey key)
        {
            return await CheckJobIsExist(key) ? await Sched.GetTriggersOfJob(key) : null;
        }

        /// <summary>
        /// 获取 指定 TriggerGroup 里的所有 Trigger
        /// </summary>
        /// <param name="triggerGroupName"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<ITrigger>> GetTriggersByGroup(string triggerGroupName)
        {
            var triggerList = new List<ITrigger>();
            var keys = await GetTriggerKeys(triggerGroupName);
            foreach (var triggerKey in keys)
            {
                triggerList.Add(await GetTrigger(triggerKey));
            }
            return triggerList;
        }

        /// <summary>
        /// 获取所有的 Trigger
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<ITrigger>> GetTriggers()
        {
            var triggerList = new List<ITrigger>();
            var triggerGroupNames = await GetTriggerGroupNames();
            foreach (var name in triggerGroupNames)
            {
                triggerList.AddRange(await GetTriggersByGroup(name));
            }

            return triggerList;
        }

        /// <summary>
        /// 获取暂停的 Trigger
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<ITrigger>> GetPausedTriggers()
        {
            var triggerList = new List<ITrigger>();
            var triggerGroupNames = await GetPausedTriggerGroupNames();
            foreach (var name in triggerGroupNames)
            {
                triggerList.AddRange(await GetTriggersByGroup(name));
            }

            return triggerList;
        }

        /// <summary>
        /// 获取 所有的 TriggerGroupName
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<string>> GetTriggerGroupNames()
        {
            return await Sched.GetTriggerGroupNames();
        }

        /// <summary>
        /// 获取 暂停的 TriggerGroupName
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<string>> GetPausedTriggerGroupNames()
        {
            return await Sched.GetPausedTriggerGroups();
        }

        /// <summary>
        /// 获取 指定 Trigger 的 TriggerState
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<TriggerState> GetTriggerState(TriggerKey key)
        {
            return await CheckTriggerIsExist(key) ? await Sched.GetTriggerState(key) : TriggerState.None;
        }

        /// <summary>
        /// 根据参数数组(1个|4个)创建SimpleTrigger
        /// </summary>
        /// <param name="triggerStr"></param>
        /// <param name="triggerName"></param>
        /// <param name="triggerGroupName"></param>
        /// <returns></returns>
        public async Task<ITrigger> GetSimpleTrigger(string triggerStr, string triggerName = null, string triggerGroupName = null)
        {
            ITrigger trigger;
            if (string.IsNullOrEmpty(triggerName)) triggerName = TriggerName;
            if (string.IsNullOrEmpty(triggerGroupName)) triggerGroupName = TriggerName;
            if (await CheckTriggerIsExist(triggerName, triggerGroupName))
            {
                ErrorMsg = "Trigger[" + triggerGroupName + "." + triggerName + "] Is  Exist";
                return null;
            }
            string[] triggerArr = triggerStr.Split('|');
            #region Length = 1
            if (triggerArr.Length == 1)
            {
                ErrorMsg = "参数 [1]TriggerArray 有误，不能解析。";
                if (!DateTimeOffset.TryParse(triggerArr[0], out var startTime)) return null;
                //int start, startUnit;
                //if (!int.TryParse(triggerArr[0], out start)) return false;
                //if (!int.TryParse(triggerArr[1], out startUnit)) return false;
                //DateTimeOffset startTime = DateBuilder.FutureDate(start, (IntervalUnit)startUnit);
                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime).Build();
                _log.InfoFormat("Trigger----{0}----{1}----", triggerArr[0], triggerArr[1]);
                ErrorMsg = "";
                return trigger;
            }
            #endregion

            #region Length = 4
            if (triggerArr.Length == 4)
            {
                ErrorMsg = "参数 [4]TriggerArray 有误，不能解析。";
                DateTimeOffset startTime;
                int interval, intervalUnit, repeatCount;
                if (!DateTimeOffset.TryParse(triggerArr[0], out startTime)) return null;
                if (!int.TryParse(triggerArr[1], out interval)) return null;
                if (!int.TryParse(triggerArr[2], out intervalUnit)) return null;
                // = DateBuilder.FutureDate(future, (IntervalUnit)futureUnit);

                if (string.IsNullOrEmpty(triggerArr[3]))
                    repeatCount = 0;
                else if (!int.TryParse(triggerArr[3], out repeatCount)) return null;
                switch (repeatCount)
                {
                    case 0:
                        switch (intervalUnit)
                        {
                            default:
                                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime)
                                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(interval).RepeatForever()).Build();
                                break;
                            case 1:
                                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime)
                                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(interval).RepeatForever()).Build();
                                break;
                            case 2:
                                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime)
                                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(interval).RepeatForever()).Build();
                                break;
                            case 3:
                                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime)
                                    .WithSimpleSchedule(x => x.WithIntervalInHours(interval).RepeatForever()).Build();
                                break;
                        }
                        break;
                    default:
                        repeatCount = repeatCount - 1;
                        switch (intervalUnit)
                        {
                            default:
                                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime)
                                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(interval).WithRepeatCount(repeatCount)).Build();
                                break;
                            case 1:
                                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime)
                                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(interval).WithRepeatCount(repeatCount)).Build();
                                break;
                            case 2:
                                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime)
                                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(interval).WithRepeatCount(repeatCount)).Build();
                                break;
                            case 3:
                                trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                    .WithIdentity(triggerName, triggerGroupName).StartAt(startTime)
                                    .WithSimpleSchedule(x => x.WithIntervalInHours(interval).WithRepeatCount(repeatCount)).Build();
                                break;
                        }
                        break;
                }
                _log.InfoFormat("Trigger----{0}----{1}----{2}----{3}----", triggerArr[0], triggerArr[1], triggerArr[2],
                    triggerArr[3]);
                ErrorMsg = "";
                return trigger;
            }
            #endregion

            ErrorMsg = "参数 TriggerArray 有误，不能解析。";
            return null;
        }

        /// <summary>
        /// 根据表达式创建CronTrigger
        /// </summary>
        /// <param name="cronExpStr"></param>
        /// <param name="triggerName"></param>
        /// <param name="triggerGroupName"></param>
        /// <returns></returns>
        public async Task<ITrigger> GetCronTrigger(string cronExpStr, string triggerName = null, string triggerGroupName = null)
        {
            ITrigger trigger = null;
            if (string.IsNullOrEmpty(triggerName)) triggerName = TriggerName;
            if (string.IsNullOrEmpty(triggerGroupName)) triggerGroupName = TriggerName;
            if (await CheckTriggerIsExist(triggerName, triggerGroupName))
            {
                ErrorMsg = "Trigger[" + triggerGroupName + "." + triggerName + "] Is  Exist";
                return null;
            }
            try
            {
                trigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithIdentity(triggerName, triggerGroupName).WithCronSchedule(cronExpStr).Build();
                return trigger;
            }
            catch (Exception e)
            {
                _log.Error(e.Message);
                ErrorMsg = e.Message;
            }
            return trigger;
        }

        /// <summary>
        /// 根据参数 创建 Cron表达式
        /// </summary>
        /// <param name="triggerStr">["*y*M*d*H*m*s"]</param>
        /// <returns></returns>
        public string GetCronExpString(string triggerStr)
        {
            try
            {
                int s = triggerStr.IndexOf("s", StringComparison.Ordinal);
                int e = triggerStr.IndexOf("m", StringComparison.Ordinal) + 1;
                string sec = ConvertSpaceStr(triggerStr.Substring(e, s - e));
                s = e - 1;
                e = triggerStr.IndexOf("H", StringComparison.Ordinal) + 1;
                string min = ConvertSpaceStr(triggerStr.Substring(e, s - e));
                s = e - 1;
                e = triggerStr.IndexOf("d", StringComparison.Ordinal) + 1;
                string hour = ConvertSpaceStr(triggerStr.Substring(e, s - e));
                s = e - 1;
                e = triggerStr.IndexOf("M", StringComparison.Ordinal) + 1;
                string day = ConvertSpaceStr(triggerStr.Substring(e, s - e));
                s = e - 1;
                e = triggerStr.IndexOf("y", StringComparison.Ordinal) + 1;
                string month = ConvertSpaceStr(triggerStr.Substring(e, s - e));
                s = e - 1;
                e = 0;
                string year = ConvertSpaceStr(triggerStr.Substring(e, s - e));
                var ss = sec + " " + min + " " + hour + " " + day + " " + month + " ? " + year + "";
                return ss;
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }

            return "";
            /*****cron表达式说明

			*    六个或七个单元
			* 秒 分 时 月中天 月份 月中星期几 (年)
			* 秒，分，时，天
			* 字段 允许值 允许的特殊字符
			秒 0-59 , - * /
			分 0-59 , - * /
			小时 0-23 , - * /
			日期 1-31 , - * ? / L W C
			月份 1-12 或者 JAN-DEC , - * /
			星期 1-7 或者 SUN-SAT , - * ? / L C #
			年（可选） 留空, 1970-2099 , - * / 

			----> 符号说明
			*：表示任意时刻
			?：只能在日或周字段上使用，简单的理解就是日期和星期是有冲突的，指定其中一个的话，另外一个是没办法指定的，比如每个月12号和每个星期二，这两个是"互斥"的，不能用日期和周来指定所有“每个是星期二的12号”这个时间。
			-：范围，如 1-5秒
			,：列表，如 1,5,10 秒
			/：等步长序列，如3/13秒 表示 3,16,29,42,55,3,16...
			L：仅在日和周上支持，表示允许的最后一个值，注意不要让范围和列表与L连用
			W：工作日
			#：为给定月份指定具体的工作日实例。把“MON#2”放在周内日期字段中，表示把任务安排在当月的第二个星期一。 

			*****/
            //秒 分 时 月中天 月份 月中星期几 (年)
        }
        /// <summary>
        /// 根据参数 创建 Cron表达式
        /// </summary>
        /// <param name="triggerArr"></param>
        /// <returns></returns>
        public string GetCronExpString(string[] triggerArr)
        {
            if (triggerArr.Length != 6)
                return "";
            return ConvertSpaceStr(triggerArr[5]) + " " + ConvertSpaceStr(triggerArr[4]) + " " + ConvertSpaceStr(triggerArr[3]) + " " + ConvertSpaceStr(triggerArr[2]) + " " + ConvertSpaceStr(triggerArr[1]) + " ? " + ConvertSpaceStr(triggerArr[0]) + "";
        }

        /// <summary>
        /// 空字符串转换
        /// </summary>
        /// <param name="triggerStr"></param>
        /// <returns></returns>
        private string ConvertSpaceStr(string triggerStr)
        {
            return string.IsNullOrEmpty(triggerStr) ? "0" : triggerStr;
        }

        /// <summary>
        /// 获取下次运行时间。
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public string GetNextDate(ITrigger trigger)
        {
            var nextFireTime = trigger.GetNextFireTimeUtc();
            DateTime nextTime;
            string nextTimeStr = "";
            if (DateTime.TryParse(nextFireTime.ToString(), out nextTime))
                nextTimeStr = nextTime.ToString("yyyy-MM-dd HH:mm:ss");
            return nextTimeStr;
        }

        #endregion

        #region Pasued / Resume

        /// <summary>
        /// 暂停 Job
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> PausedJob(JobKey key)
        {
            if (!await CheckJobIsExist(key))
            {
                ErrorMsg = key + " is not find!";
                return false;
            }
            await Sched.PauseJob(key);
            return true;
        }
        /// <summary>
        /// 恢复 Job
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ResumeJob(JobKey key)
        {
            if (!await CheckJobIsExist(key))
            {
                ErrorMsg = key + " is not find!";
                return false;
            }
            await Sched.PauseJob(key);
            return true;
        }

        /// <summary>
        /// 暂停 Job
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <returns></returns>
        public async Task<bool> PausedJob(IJobDetail jobDetail)
        {
            return await PausedJob(jobDetail.Key);
        }
        /// <summary>
        /// 恢复 Job
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <returns></returns>
        public async Task<bool> ResumeJob(IJobDetail jobDetail)
        {
            return await ResumeJob(jobDetail.Key);
        }

        /// <summary>
        ///  暂停 Job组
        /// </summary>
        /// <param name="jobGroupName"></param>
        public async Task PausedJobs(string jobGroupName)
        {
            await Sched.PauseJobs(GroupMatcher<JobKey>.GroupEquals(jobGroupName));
        }
        /// <summary>
        ///  恢复 Job组
        /// </summary>
        /// <param name="jobGroupName"></param>
        public async Task ResumeJobs(string jobGroupName)
        {
            await Sched.PauseJobs(GroupMatcher<JobKey>.GroupEquals(jobGroupName));
        }

        /// <summary>
        /// 暂停 Trigger
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> PausedTrigger(TriggerKey key)
        {
            if (!await CheckTriggerIsExist(key))
            {
                ErrorMsg = key + " is not find!";
                return false;
            }
            await Sched.PauseTrigger(key);
            return true;
        }
        /// <summary>
        /// 恢复 Trigger
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ResumeTrigger(TriggerKey key)
        {
            if (!await CheckTriggerIsExist(key))
            {
                ErrorMsg = key + " is not find!";
                return false;
            }
            await Sched.PauseTrigger(key);
            return true;
        }

        /// <summary>
        /// 暂停 Trigger
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public async Task<bool> PausedTrigger(ITrigger trigger)
        {
            return await PausedTrigger(trigger.Key);
        }
        /// <summary>
        /// 恢复 Trigger
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public async Task<bool> ResumeTrigger(ITrigger trigger)
        {
            return await ResumeTrigger(trigger.Key);
        }

        /// <summary>
        /// 暂停 Trigger组
        /// </summary>
        /// <param name="triggerGroupName"></param>
        public async Task PausedTriggers(string triggerGroupName)
        {
            await Sched.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(triggerGroupName));
        }
        /// <summary>
        /// 恢复 Trigger组
        /// </summary>
        /// <param name="triggerGroupName"></param>
        public async Task ResumeTriggers(string triggerGroupName)
        {
            await Sched.PauseTriggers(GroupMatcher<TriggerKey>.GroupEquals(triggerGroupName));
        }

        /// <summary>
        /// 暂停调度中所有的job任务
        /// </summary>
        public void PausedAll()
        {
            Sched.PauseAll();
        }
        /// <summary>
        /// 恢复调度中所有的job任务
        /// </summary>
        public void ResumeAll()
        {
            Sched.PauseAll();
        }

        #endregion

        #region Get Key

        /// <summary>
        /// 获取 Jobkey
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="jobGroupName"></param>
        /// <returns></returns>
        public JobKey GetJobKey(string jobName, string jobGroupName)
        {
            return JobKey.Create(jobName, jobGroupName);
        }
        /// <summary>
        /// 获取 JobKeys
        /// </summary>
        /// <param name="jobGroupName"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<JobKey>> GetJobKeys(string jobGroupName)
        {
            return await Sched.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(jobGroupName));
        }

        /// <summary>
        /// 获取 TriggerKey
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="triggerGroupName"></param>
        /// <returns></returns>
        public TriggerKey GetTriggerKey(string triggerName, string triggerGroupName)
        {
            return new TriggerKey(triggerName, triggerGroupName);
        }
        /// <summary>
        /// 获取 TriggerKeys
        /// </summary>
        /// <param name="triggerGroupName"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<TriggerKey>> GetTriggerKeys(string triggerGroupName)
        {
            return await Sched.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(triggerGroupName));
        }

        #endregion

        #region Check IsExist

        /// <summary>
        /// 检查调度是否启动
        /// </summary>
        /// <returns></returns>
        public bool CheckIsStarted()
        {
            return Sched.IsStarted;
        }

        /// <summary>
        /// 检查Job是否已存在
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="jobGroupName"></param>
        /// <returns></returns>
        public async Task<bool> CheckJobIsExist(string jobName, string jobGroupName)
        {
            JobKey key = new JobKey(jobName, jobGroupName);
            return await CheckJobIsExist(key);
        }
        /// <summary>
        /// 检查Job是否已存在;
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <returns></returns>
        public async Task<bool> CheckJobIsExist(IJobDetail jobDetail)
        {
            return await CheckJobIsExist(jobDetail.Key);
        }
        /// <summary>
        /// 检查Job是否已存在;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> CheckJobIsExist(JobKey key)
        {
            return await Sched.CheckExists(key);
        }

        /// <summary>
        /// 检查Trigger是否已存在;
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="triggerGroupName"></param>
        /// <returns></returns>
        public async Task<bool> CheckTriggerIsExist(string triggerName, string triggerGroupName)
        {
            TriggerKey key = new TriggerKey(triggerName, triggerGroupName);
            return await CheckTriggerIsExist(key);
        }
        /// <summary>
        /// 检查Trigger是否已存在;
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public async Task<bool> CheckTriggerIsExist(ITrigger trigger)
        {
            return await CheckJobIsExist(trigger.JobKey);
        }
        /// <summary>
        /// 检查Trigger是否已存在;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> CheckTriggerIsExist(TriggerKey key)
        {
            return await Sched.CheckExists(key);
        }

        #endregion

        #region Start / Stop

        /// <summary>
        /// 启动调度信息
        /// </summary>
        public void Start()
        {
            _log.Info("------- Starting ---------------------]");
            Sched.Start();
            _log.Info("------- Started ---------------------]");
        }
        /// <summary>
        /// 关闭调度信息
        /// </summary>
        public void Shutdown()
        {
            _log.Info("------- Shutting Down ---------------------");

            Sched.Shutdown(true);

            _log.Info("------- Shutdown Complete -----------------");
        }

        /// <summary>
        /// 启动 任务
        /// </summary>
        /// <param name="jobkey"></param>
        public void TriggerJob(JobKey jobkey)
        {
            Sched.TriggerJob(jobkey);
        }
        /// <summary>
        /// 启动 任务
        /// </summary>
        /// <param name="jobkey"></param>
        /// <param name="jobdatamap"></param>
        public void TriggerJob(JobKey jobkey, JobDataMap jobdatamap)
        {
            Sched.TriggerJob(jobkey, jobdatamap);
        }

        #endregion

        #region Reset Name

        /// <summary>
        /// 重置 jobName
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="jobGroupName"></param>
        public void ResetJobName(string jobName = null, string jobGroupName = null)
        {
            if (!string.IsNullOrEmpty(jobName))
                JobName = jobName;
            if (!string.IsNullOrEmpty(jobGroupName))
                JobGroupName = jobGroupName;
        }
        /// <summary>
        /// 重置 TriggerName
        /// </summary>
        /// <param name="triggerName"></param>
        /// <param name="triggerGroupName"></param>
        public void ResetTriggerName(string triggerName = null, string triggerGroupName = null)
        {
            if (!string.IsNullOrEmpty(triggerName))
                TriggerName = triggerName;
            if (!string.IsNullOrEmpty(triggerGroupName))
                TriggerGroupName = triggerGroupName;
        }

        #endregion

    }

    public enum TriggerType
    {
        SimpleTrigger = 1,
        CronTrigger = 2
    }


}
