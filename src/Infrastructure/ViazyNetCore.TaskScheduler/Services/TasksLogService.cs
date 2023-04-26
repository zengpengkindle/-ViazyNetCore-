using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using FreeSql;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ViazyNetCore.TaskScheduler
{
    public class TasksLogService
    {
        private readonly IBaseRepository<TaskLog, long> _taskLogRepository;

        public TasksLogService(IBaseRepository<TaskLog, long> taskLogRepository)
        {
            this._taskLogRepository = taskLogRepository;
        }

        public async Task InsertAsync(TaskLog tasksLog)
        {
            await this._taskLogRepository.InsertAsync(tasksLog);
        }

        public async Task<PageData<TaskLog>> GetTaskLogs(Pagination pagination, int jobId, DateTime? runTime, DateTime? endTime)
        {
           return  await this._taskLogRepository.Select.From<TaskInfo>()
               .LeftJoin((log, qz) => log.JobId == qz.Id)
               .OrderByDescending((log,qz) => log.RunTime)
               .WhereIf(jobId > 0, (log,qz) => log.JobId == jobId)
               .WhereIf(runTime != null, (log, qz) => log.RunTime >= runTime)
               .WhereIf(endTime != null, (log, qz) => log.RunTime <= endTime)
               .WithTempQuery((log, qz) => new TaskLog
               {
                   RunPars = log.RunPars,
                   RunResult = log.RunResult,
                   RunTime = log.RunTime,
                   EndTime = log.EndTime,
                   ErrMessage = log.ErrMessage,
                   ErrStackTrace = log.ErrStackTrace,
                   TotalTime = log.TotalTime,
                   Name = qz.Name,
                   JobGroup = qz.JobGroup
               })
               .ToPageAsync(pagination);
        }
    }
}
