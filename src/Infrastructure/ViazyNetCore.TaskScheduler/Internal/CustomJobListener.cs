using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ViazyNetCore.TaskScheduler
{
    /// <summary>
    /// 可以做些记录 
    /// </summary>
    public class CustomJobListener : IJobListener
    {
        private readonly TasksLogService? _tasksLogService;
        private readonly TasksQzService? _tasksQzService;
        public string Name => "CustomJobListener";

        public CustomJobListener(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            this._tasksQzService = scope.ServiceProvider.GetService<TasksQzService>();
            this._tasksLogService = scope.ServiceProvider.GetService<TasksLogService>();
        }

        /// <summary>
        /// 执行中断  触发
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                Console.WriteLine("this is JobExecutionVetoed");
            });
        }

        /// <summary>
        /// 即将被执行 触发
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                JobDataMap triggerPars = context.Trigger.JobDataMap;
                triggerPars.Add("#Trigger:BeginTime", DateTime.Now.ToJsTime());
                Console.WriteLine("this is JobToBeExecuted");
            });
        }

        /// <summary>
        /// 执行完毕后  触发
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jobException"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            //记录Job
            TaskLog tasksLog = new TaskLog();
            //JOBID
            long jobid = context.JobDetail.Key.Name.ParseTo<long>();
            //JOB组名
            string groupName = context.JobDetail.Key.Group;
            JobDataMap triggerPars = context.Trigger.JobDataMap;
            var triggerBeginTime = triggerPars.GetLong("#Trigger:BeginTime");
            tasksLog.RunTime = triggerBeginTime.FromJsTime();
            //日志
            tasksLog.JobId = jobid;
            string jobHistory = $"【{tasksLog.RunTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行开始】【Id：{jobid}，组别：{groupName}】";

            tasksLog.EndTime = DateTime.Now;
            tasksLog.RunResult = true;
            jobHistory += $"，【{tasksLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行成功】";

            JobDataMap jobPars = context.JobDetail.JobDataMap;
            tasksLog.RunPars = jobPars.GetString("JobParam");
            tasksLog.TotalTime = Math.Round((tasksLog.EndTime - tasksLog.RunTime).TotalSeconds, 3);
            jobHistory += $"(耗时:{tasksLog.TotalTime}秒)";
            if (this._tasksQzService != null)
            {
                var model = await this._tasksQzService.GetByIdAsync(jobid);
                if (model != null)
                {
                    if (_tasksLogService != null) await this._tasksLogService.InsertAsync(tasksLog);
                    model.RunTimes += 1;
                    if (model.TriggerType == 0) model.CycleHasRunTimes += 1;
                    if (model.TriggerType == 0 && model.CycleRunTimes != 0 && model.CycleHasRunTimes >= model.CycleRunTimes) model.IsStart = false;//循环完善,当循环任务完成后,停止该任务,防止下次启动再次执行
                    var separator = "<br>";
                    // 这里注意数据库字段的长度问题，超过限制，会造成数据库remark不更新问题。
                    model.Remark =
                        $"{jobHistory}{separator}" + string.Join(separator, UnitHelper.GetTopDataBySeparator(model.Remark, separator, 9));
                    await this._tasksQzService.Update(model);
                }
            }
        }
    }

}
