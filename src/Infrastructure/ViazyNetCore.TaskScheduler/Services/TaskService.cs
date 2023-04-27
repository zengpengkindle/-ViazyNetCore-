using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using FreeSql;
using Quartz.Util;

namespace ViazyNetCore.TaskScheduler
{
    public class TaskService
    {
        private readonly IBaseRepository<TaskInfo, long> _taskQzRepository;

        public TaskService(IBaseRepository<TaskInfo, long> taskQzRepository)
        {
            this._taskQzRepository = taskQzRepository;
        }

        public Task<TaskInfo> GetByIdAsync(long jobId)
        {
            return this._taskQzRepository.GetAsync(jobId);
        }

        public Task Update(TaskInfo model)
        {
            return this._taskQzRepository.UpdateAsync(model);
        }

        public Task<PageData<TaskInfo>> QueryPageList(Pagination pagination, string key)
        {
            return this._taskQzRepository.Where(p => !p.IsDeleted).WhereIf(key.IsNotNull(), p => p.Name.Contains(key))
                .OrderByDescending(p => p.CreateTime)
                 .ToPageAsync(pagination);
        }

        public async Task InsertAsync(TaskInfo tasksQz)
        {
            await this._taskQzRepository.InsertAsync(tasksQz);
        }

        public async Task DeleteAsync(long taskId)
        {
            await this._taskQzRepository.DeleteAsync(taskId);
        }

        public Task<List<TaskInfo>> GetAllStart()
        {
            return this._taskQzRepository.Where(p => !p.IsDeleted && p.IsStart).ToListAsync();
        }
    }
}
