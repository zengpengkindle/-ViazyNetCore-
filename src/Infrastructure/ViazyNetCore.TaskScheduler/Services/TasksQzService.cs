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
    public class TasksQzService
    {
        private readonly IBaseRepository<TasksQz, long> _taskQzRepository;

        public TasksQzService(IBaseRepository<TasksQz, long> taskQzRepository)
        {
            this._taskQzRepository = taskQzRepository;
        }

        public Task<TasksQz> GetByIdAsync(long jobId)
        {
            return this._taskQzRepository.GetAsync(jobId);
        }

        public Task Update(TasksQz model)
        {
            return this._taskQzRepository.UpdateAsync(model);
        }

        public Task<PageData<TasksQz>> QueryPageList(Pagination pagination, string key)
        {
            return this._taskQzRepository.Where(p => !p.IsDeleted).WhereIf(key.IsNotNull(), p => p.Name.Contains(key))
                .OrderByDescending(p => p.CreateTime)
                 .ToPageAsync(pagination);
        }

        public async Task InsertAsync(TasksQz tasksQz)
        {
            await this._taskQzRepository.InsertAsync(tasksQz);
        }

        public async Task DeleteAsync(long taskId)
        {
            await this._taskQzRepository.DeleteAsync(taskId);
        }

        public Task<List<TasksQz>> GetAllStart()
        {
            return this._taskQzRepository.Where(p => !p.IsDeleted && p.IsStart).ToListAsync();
        }
    }
}
