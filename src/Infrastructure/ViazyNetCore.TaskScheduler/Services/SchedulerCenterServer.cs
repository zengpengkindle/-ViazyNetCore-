using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quartz.Impl.Triggers;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using System.Threading;

namespace ViazyNetCore.TaskScheduler
{
    public class SchedulerCenterServer : ISchedulerCenter
    {
        private Lazy<IScheduler> _scheduler;
        private readonly IJobFactory _iocjobFactory;
        private readonly IServiceProvider _serviceProvider;

        public SchedulerCenterServer(IJobFactory jobFactory, IServiceProvider serviceProvider)
        {
            this._iocjobFactory = jobFactory;
            this._serviceProvider = serviceProvider;
            this._scheduler = new Lazy<IScheduler>(() => GetSchedulerAsync());
        }
        private IScheduler GetSchedulerAsync()
        {
            // 从Factory中获取Scheduler实例
            NameValueCollection collection = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" },
                };
            var factory = new StdSchedulerFactory(collection);
            var scheduler = factory.GetScheduler().GetAwaiter().GetResult();
            scheduler.ListenerManager.AddSchedulerListener(new CustomSchedulerListener());
            scheduler.ListenerManager.AddJobListener(new CustomJobListener(this._serviceProvider));
            return scheduler;
        }

        private Task<IScheduler> GetSchedulerAsync(string groupName)
        {
            // 从Factory中获取Scheduler实例
            NameValueCollection collection = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" },
                    { "quartz.scheduler.instanceName",groupName}
                };
            var factory = new StdSchedulerFactory(collection);
            return factory.GetScheduler(groupName);
        }

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StartScheduleAsync()
        {
            try
            {
                this._scheduler.Value.JobFactory = this._iocjobFactory;
                if (!this._scheduler.Value.IsStarted)
                {
                    //等待任务运行完成
                    await this._scheduler.Value.Start();
                    await Console.Out.WriteLineAsync("任务调度开启！");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StopScheduleAsync()
        {
            try
            {
                if (!this._scheduler.Value.IsShutdown)
                {
                    //等待任务运行完成
                    await this._scheduler.Value.Shutdown();
                    await Console.Out.WriteLineAsync("任务调度停止！");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加一个计划任务（映射程序集指定IJob实现类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task AddScheduleJobAsync(TaskInfo tasksQz)
        {
            if (tasksQz != null)
            {
                try
                {
                    JobKey jobKey = new JobKey(tasksQz.Id.ToString(), tasksQz.JobGroup);
                    if (await _scheduler.Value.CheckExists(jobKey) && tasksQz.TriggerType > 0)
                    {
                        throw new ApiException($"该任务计划已经在执行:【{tasksQz.Name}】,请勿重复启动！");
                    }
                    if (tasksQz.TriggerType == 0 && tasksQz.CycleRunTimes != 0 && tasksQz.CycleHasRunTimes >= tasksQz.CycleRunTimes)
                    {
                        //result.success = false;
                        //result.msg = $"该任务计划已完成:【{tasksQz.Name}】,无需重复启动,如需启动请修改已循环次数再提交";
                        //return result;
                        throw new ApiException($"该任务计划已完成:【{tasksQz.Name}】,无需重复启动,如需启动请修改已循环次数再提交");
                    }
                    #region 设置开始时间和结束时间

                    if (tasksQz.BeginTime == null)
                    {
                        tasksQz.BeginTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(tasksQz.BeginTime, 1);//设置开始时间
                    if (tasksQz.EndTime == null)
                    {
                        tasksQz.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(tasksQz.EndTime, 1);//设置暂停时间

                    #endregion

                    #region 通过反射获取程序集类型和类   

                    Assembly assembly = Assembly.Load(new AssemblyName(tasksQz.AssemblyName));
                    Type jobType = assembly.GetType(tasksQz.ClassName);

                    #endregion
                    //判断任务调度是否开启
                    if (!_scheduler.Value.IsStarted)
                    {
                        await this.StartScheduleAsync();
                    }

                    //传入反射出来的执行程序集
                    IJobDetail job = new JobDetailImpl(tasksQz.Id.ToString(), tasksQz.JobGroup, jobType, false, false);
                    job.JobDataMap.Add("JobParam", tasksQz.JobParams);
                    ITrigger trigger;

                    #region 泛型传递
                    //IJobDetail job = JobBuilder.Create<T>()
                    //    .WithIdentity(sysSchedule.Name, sysSchedule.JobGroup)
                    //    .Build();
                    #endregion

                    if (tasksQz.Cron != null && CronExpression.IsValidExpression(tasksQz.Cron) && tasksQz.TriggerType > 0)
                    {
                        trigger = CreateCronTrigger(tasksQz);

                        ((CronTriggerImpl)trigger).MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing;

                        // 告诉Quartz使用我们的触发器来安排作业
                        await _scheduler.Value.ScheduleJob(job, trigger);
                    }
                    else
                    {
                        List<ITrigger> triggers = new List<ITrigger>();
                        if (tasksQz.TriggerCount <= 0)
                        {
                            tasksQz.TriggerCount = 1;
                        }
                        for (var i = 0; i < tasksQz.TriggerCount; i++)
                        {
                            triggers.Add(CreateSimpleTrigger(tasksQz));
                        }
                        // 告诉Quartz使用我们的触发器来安排作业
                        await _scheduler.Value.ScheduleJob(job, triggers, replace: true);
                    }
                    //await Task.Delay(TimeSpan.FromSeconds(120));
                    //await Console.Out.WriteLineAsync("关闭了调度器！");
                    //await _scheduler.Value.Shutdown();
                }
                catch (Exception ex)
                {
                    throw new ApiException($"任务计划异常:【{ex.Message}】");
                }
            }
            else
            {
                throw new ApiException($"任务计划不存在:【{tasksQz?.Name}】");
            }
        }

        /// <summary>
        /// 任务是否存在?
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsExistScheduleJobAsync(TaskInfo sysSchedule)
        {
            JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
            if (await _scheduler.Value.CheckExists(jobKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        public async Task StopScheduleJobAsync(TaskInfo sysSchedule)
        {
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Value.CheckExists(jobKey))
                {
                    //throw new ApiException($"未找到要暂停的任务:【{sysSchedule.Name}】");
                    return;
                }
                else
                {
                    var jobDetail = await this._scheduler.Value.GetJobDetail(jobKey);
                    if (jobDetail != null && typeof(IInterruptableJob).IsAssignableFrom(jobDetail.JobType))
                    {
                        await this._scheduler.Value.Interrupt(jobKey);// 先任务终止
                    }
                    await this._scheduler.Value.DeleteJob(jobKey);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 恢复指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        public async Task ResumeJob(TaskInfo sysSchedule)
        {
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Value.CheckExists(jobKey))
                {
                    throw new ApiException($"未找到要恢复的任务:【{sysSchedule.Name}】");
                }
                await this._scheduler.Value.ResumeJob(jobKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 暂停指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        public async Task PauseJob(TaskInfo sysSchedule)
        {
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Value.CheckExists(jobKey))
                {
                    throw new ApiException($"未找到要暂停的任务:【{sysSchedule.Name}】");
                }
                await this._scheduler.Value.PauseJob(jobKey);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 状态状态帮助方法
        public async Task<List<TaskInfoDto>> GetTaskStaus(TaskInfo sysSchedule)
        {

            var ls = new List<TaskInfoDto>();
            var noTask = new List<TaskInfoDto>{ new TaskInfoDto {
                JobId = sysSchedule.Id.ToString(),
                JobGroup = sysSchedule.JobGroup,
                TriggerId = "",
                TriggerGroup = "",
                TriggerStatus = "不存在"
            } };
            JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
            IJobDetail job = await this._scheduler.Value.GetJobDetail(jobKey);
            if (job == null)
            {
                return noTask;
            }
            //info.Append(string.Format("任务ID:{0}\r\n任务名称:{1}\r\n", job.Key.Name, job.Description)); 
            var triggers = await this._scheduler.Value.GetTriggersOfJob(jobKey);
            if (triggers == null || triggers.Count == 0)
            {
                return noTask;
            }
            foreach (var trigger in triggers)
            {
                var triggerStaus = await this._scheduler.Value.GetTriggerState(trigger.Key);
                string state = GetTriggerState(triggerStaus.ToString());
                ls.Add(new TaskInfoDto
                {
                    JobId = job.Key.Name,
                    JobGroup = job.Key.Group,
                    TriggerId = trigger.Key.Name,
                    TriggerGroup = trigger.Key.Group,
                    TriggerStatus = state
                });
                //info.Append(string.Format("触发器ID:{0}\r\n触发器名称:{1}\r\n状态:{2}\r\n", item.Key.Name, item.Description, state));

            }
            return ls;
        }

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        public async Task PauseScheduleTriggerAsync(TaskInfo sysSchedule, string triggerId)
        {
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Value.CheckExists(jobKey))
                {
                    return;
                }
                else
                {
                    var triggerKey = new TriggerKey(triggerId, sysSchedule.JobGroup);
                    if (await this._scheduler.Value.CheckExists(triggerKey))
                    {
                        //await this._scheduler.Value.PauseTrigger(triggerKey);
                        await this._scheduler.Value.UnscheduleJob(triggerKey);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        public async Task ResumeScheduleTriggerAsync(TaskInfo sysSchedule, string triggerId)
        {
            try
            {
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await _scheduler.Value.CheckExists(jobKey))
                {
                    return;
                }
                else
                {
                    var triggerKey = new TriggerKey(triggerId, sysSchedule.JobGroup);
                    if (await this._scheduler.Value.CheckExists(triggerKey))
                    {
                        await this._scheduler.Value.ResumeTrigger(triggerKey);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetTriggerState(string key)
        {
            string state = null;
            if (key != null)
                key = key.ToUpper();
            switch (key)
            {
                case "1":
                    state = "暂停";
                    break;
                case "2":
                    state = "完成";
                    break;
                case "3":
                    state = "出错";
                    break;
                case "4":
                    state = "阻塞";
                    break;
                case "0":
                    state = "正常";
                    break;
                case "-1":
                    state = "不存在";
                    break;
                case "BLOCKED":
                    state = "阻塞";
                    break;
                case "COMPLETE":
                    state = "完成";
                    break;
                case "ERROR":
                    state = "出错";
                    break;
                case "NONE":
                    state = "不存在";
                    break;
                case "NORMAL":
                    state = "正常";
                    break;
                case "PAUSED":
                    state = "暂停";
                    break;
            }
            return state;
        }
        #endregion

        #region 创建触发器帮助方法

        /// <summary>
        /// 创建SimpleTrigger触发器（简单触发器）
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <param name="starRunTime"></param>
        /// <param name="endRunTime"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(TaskInfo sysSchedule)
        {
            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity(Guid.NewGuid().ToString("N"), sysSchedule.JobGroup)
            .StartAt(sysSchedule.BeginTime.Value)
            .WithSimpleSchedule(x =>
            {
                if (sysSchedule.IntervalSecond > 0)
                {
                    x.WithIntervalInSeconds(sysSchedule.IntervalSecond);
                }
                if (sysSchedule.CycleRunTimes > 0)
                {
                    x.WithRepeatCount(sysSchedule.CycleRunTimes - 1);
                }
                else
                {
                    x.RepeatForever();
                }
            })
            .EndAt(sysSchedule.EndTime)
            .ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup)//作业名称
            .Build();
            return trigger;

            // 触发作业立即运行，然后每10秒重复一次，无限循环

        }
        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(TaskInfo sysSchedule)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(Guid.NewGuid().ToString("N"), sysSchedule.JobGroup)
                   .StartAt(sysSchedule.BeginTime.Value)//开始时间
                   .EndAt(sysSchedule.EndTime.Value)//结束数据
                   .WithCronSchedule(sysSchedule.Cron)//指定cron表达式
                   .ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup)//作业名称
                   .Build();
        }
        #endregion

        /// <summary>
        /// 立即执行 一个任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task ExecuteJobAsync(TaskInfo tasksQz)
        {
            try
            {
                JobKey jobKey = new JobKey(tasksQz.Id.ToString(), tasksQz.JobGroup);
                //判断任务是否存在，存在则 触发一次，不存在则先添加一个任务，触发以后再 停止任务
                if (!await _scheduler.Value.CheckExists(jobKey))
                {
                    //不存在 则 添加一个计划任务
                    await AddScheduleJobAsync(tasksQz);
                    //触发执行一次
                    await _scheduler.Value.TriggerJob(jobKey);
                    //停止任务
                    await StopScheduleJobAsync(tasksQz);
                }
                else
                {
                    await _scheduler.Value.TriggerJob(jobKey);
                }
            }
            catch (Exception ex)
            {
                throw new ApiException($"立即执行计划任务失败:【{ex.Message}】");
            }
        }
    }
}
