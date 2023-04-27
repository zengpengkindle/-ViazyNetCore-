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
    public class CustomTriggerListener : ITriggerListener
    {
        private readonly TasksLogService? _tasksLogService;
        private readonly TaskService? _tasksQzService;
        public CustomTriggerListener(IServiceProvider serviceProvider)
        {
            this._tasksQzService = serviceProvider.GetService<TaskService>();
            this._tasksLogService = serviceProvider.GetService<TasksLogService>();
        }

        public string Name => "CustomTriggerListener";

        public async Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            //记录Job
            TaskLog tasksLog = new TaskLog();
            //JOBID
            long jobid = context.JobDetail.Key.Name.ParseTo<long>();
            //JOB组名
            string groupName = context.JobDetail.Key.Group;
            //日志
            tasksLog.JobId = jobid;
            tasksLog.RunTime = DateTime.Now;
            string jobHistory = $"【{tasksLog.RunTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行开始】【Id：{jobid}，组别：{groupName}】";
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

        public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                Console.WriteLine("this is TriggerFired");
            });
        }

        public async Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                Console.WriteLine("this is TriggerMisfired");
            });
        }

        /// <summary>
        /// 当前任务 是否被取消
        /// 返回false 就继续执行
        /// 返回true就被取消了
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                Console.WriteLine("this is VetoJobExecution");
            });
            return false;
        }
    }

}
