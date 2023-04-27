using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using FreeSql.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ViazyNetCore.TaskScheduler
{
    public abstract class JobBase : IJob
    {
        private readonly TasksLogService? _tasksLogService;
        private readonly TaskService? _tasksQzService;

        public JobBase(IServiceProvider serviceProvider)
        {
            this._tasksQzService = serviceProvider.GetService<TaskService>();
            this._tasksLogService = serviceProvider.GetService<TasksLogService>();
        }

        protected abstract Task RunJob(IJobExecutionContext context);

        /// <inheritdoc/>
        public async Task Execute(IJobExecutionContext context)
        {
            await this.RunJob(context);
        }

        /// <summary>
        /// 执行指定任务
        /// </summary>
        protected virtual async Task<string> ExecuteJob(IJobExecutionContext context, Func<Task> func)
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
            try
            {
                await func();//执行任务
                tasksLog.EndTime = DateTime.Now;
                tasksLog.RunResult = true;
                jobHistory += $"，【{tasksLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行成功】";

                JobDataMap jobPars = context.JobDetail.JobDataMap;
                tasksLog.RunPars = jobPars.GetString("JobParam");
            }
            catch (Exception ex)
            {
                tasksLog.EndTime = DateTime.Now;
                tasksLog.RunResult = false;
                //JobExecutionException e2 = new JobExecutionException(ex);
                //true  是立即重新执行任务 
                //e2.RefireImmediately = true;
                tasksLog.ErrMessage = ex.Message;
                tasksLog.ErrStackTrace = ex.StackTrace;
                jobHistory += $"，【{tasksLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行失败:{ex.Message}】";
            }
            finally
            {
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

            Console.Out.WriteLine(jobHistory);
            return jobHistory;
        }
    }
}
