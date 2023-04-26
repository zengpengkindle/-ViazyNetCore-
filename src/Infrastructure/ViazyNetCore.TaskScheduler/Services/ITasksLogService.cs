using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Model.Models;

namespace ViazyNetCore.TaskScheduler
{
    public interface ITasksLogService
    {
        public Task<PageData<TaskLog>> GetTaskLogs(Pagination pagination, int jobId, DateTime? runTime, DateTime? endTime);
        public Task<object> GetTaskOverview(int jobId, DateTime? runTime, DateTime? endTime, string type);
    }
}
