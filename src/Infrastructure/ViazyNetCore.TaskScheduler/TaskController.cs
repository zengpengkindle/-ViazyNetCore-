using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using ViazyNetCore.Data.FreeSql;
using ViazyNetCore.DI;

namespace ViazyNetCore.TaskScheduler
{
    [Authorize]
    [Area("task")]
    [DynamicApi]
    public class TaskController : IDynamicController
    {
        private readonly TasksQzService _tasksQzService;
        private readonly ISchedulerCenter _schedulerCenter;
        private readonly UnitOfWorkManager _unitOfWorkManage;
        private readonly TasksLogService _tasksLogService;

        public TaskController(TasksQzService tasksQzService,
            ISchedulerCenter schedulerCenter,
            UnitOfWorkManagerCloud unitOfWorkManage,
            TasksLogService tasksLogService)
        {
            this._tasksQzService = tasksQzService;
            this._schedulerCenter = schedulerCenter;
            this._unitOfWorkManage = unitOfWorkManage.GetUnitOfWorkManager("master");
            this._tasksLogService = tasksLogService;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageData<TasksQz>> GetList([FromQuery] Pagination pagination, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            var data = await _tasksQzService.QueryPageList(pagination, key);
            if (data.Total > 0)
            {
                foreach (var item in data.Rows)
                {
                    item.Triggers = await _schedulerCenter.GetTaskStaus(item);
                }
            }
            return data;
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TasksQz> Get(long jobId)
        {
            var model = await _tasksQzService.GetByIdAsync(jobId);
            if (model == null)
                throw new ApiException("任务不存在");

            model.Triggers = await _schedulerCenter.GetTaskStaus(model);
            return model;
        }

        /// <summary>
        /// 添加计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddTask([Required][FromBody] TasksQzEditDto tasksQzDto)
        {
            //var request = await this._httpContextAccessor.HttpContext.Request.Body.ReadToEndAsync();
            using var unow = _unitOfWorkManage.Begin();
            var tasksQz = tasksQzDto.CopyTo<TasksQz>();
            await _tasksQzService.InsertAsync(tasksQz);
            try
            {
                if (tasksQz.IsStart)
                {
                    //如果是启动自动
                    await _schedulerCenter.AddScheduleJobAsync(tasksQz);
                }
                unow.Commit();
            }
            catch (Exception)
            {
                unow.Rollback();
                throw;
            }
        }


        /// <summary>
        /// 修改计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task UpdateTask([FromBody] TasksQzEditDto tasksQzDto)
        {
            var tasksQz = tasksQzDto.CopyTo<TasksQz>();
            if (tasksQz != null && tasksQz.Id > 0)
            {
                using var unow = _unitOfWorkManage.Begin();
                await _tasksQzService.Update(tasksQz);
                try
                {

                    if (tasksQz.IsStart)
                    {
                        await _schedulerCenter.StopScheduleJobAsync(tasksQz);
                        await _schedulerCenter.AddScheduleJobAsync(tasksQz);
                    }
                    else
                    {
                        await _schedulerCenter.StopScheduleJobAsync(tasksQz);
                    }
                    unow.Commit();
                }
                catch (Exception)
                {
                    unow.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 删除一个任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task Delete(long jobId)
        {
            var model = await _tasksQzService.GetByIdAsync(jobId);
            if (model != null)
            {
                using var unow = _unitOfWorkManage.Begin();
                await _tasksQzService.DeleteAsync(jobId);
                try
                {
                    await _schedulerCenter.StopScheduleJobAsync(model);
                    unow.Commit();
                }
                catch (Exception)
                {
                    unow.Rollback();
                    throw;
                }
            }
            else
            {
                throw new ApiException("任务不存在");
            }

        }
        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task StartJob(long jobId)
        {
            var model = await _tasksQzService.GetByIdAsync(jobId);
            if (model != null)
            {
                using var unow = _unitOfWorkManage.Begin();
                try
                {
                    model.IsStart = true;
                    await _tasksQzService.Update(model);
                    await _schedulerCenter.AddScheduleJobAsync(model);
                    unow.Commit();
                }
                catch (Exception)
                {
                    unow.Rollback();
                    throw;
                }
            }
            else
            {
                throw new ApiException("任务不存在");
            }
        }
        /// <summary>
        /// 停止一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>        
        [HttpGet]
        public async Task StopJob(long jobId)
        {
            var model = await _tasksQzService.GetByIdAsync(jobId);
            if (model != null)
            {
                model.IsStart = false;
                await _tasksQzService.Update(model);
                await _schedulerCenter.StopScheduleJobAsync(model);
            }
            else
            {
                throw new ApiException("任务不存在");
            }
        }
        /// <summary>
        /// 暂停一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>        
        [HttpGet]
        public async Task PauseJob(long jobId)
        {
            var model = await _tasksQzService.GetByIdAsync(jobId);
            if (model != null)
            {
                using var unow = _unitOfWorkManage.Begin();
                try
                {
                    await _tasksQzService.Update(model);
                    jobId.ToString();
                    await _schedulerCenter.PauseJob(model);
                    unow.Commit();
                }
                catch (Exception)
                {
                    unow.Rollback();
                    throw;
                }
            }
            else
            {
                throw new ApiException("任务不存在");
            }
        }
        /// <summary>
        /// 恢复一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>        
        [HttpGet]
        public async Task ResumeJob(long jobId)
        {
            var model = await _tasksQzService.GetByIdAsync(jobId);
            if (model != null)
            {
                using var unow = _unitOfWorkManage.Begin();
                try
                {
                    model.IsStart = true;
                    await _tasksQzService.Update(model);
                    await _schedulerCenter.ResumeJob(model);
                    unow.Commit();
                }
                catch (Exception)
                {
                    unow.Rollback();
                    throw;
                }
            }
            else
            {
                throw new ApiException("任务不存在");
            }
        }
        /// <summary>
        /// 重启一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task ReCovery(long jobId)
        {
            var model = await _tasksQzService.GetByIdAsync(jobId);
            if (model != null)
            {
                using var unow = _unitOfWorkManage.Begin();
                try
                {
                    model.IsStart = true;
                    await _tasksQzService.Update(model);

                    await _schedulerCenter.StopScheduleJobAsync(model);
                    await _schedulerCenter.AddScheduleJobAsync(model);
                    unow.Commit();
                }
                catch (Exception)
                {
                    unow.Rollback();
                    throw;
                }
            }
            else
            {
                throw new ApiException("任务不存在");
            }

        }
        /// <summary>
        /// 获取任务命名空间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<QuartzReflectionItem> GetTaskNameSpace()
        {
            var baseType = typeof(IJob);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, "ViazyNetCore.Tasks.dll").Select(Assembly.LoadFrom).ToArray();
            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => x.IsClass).Select(item => new QuartzReflectionItem { NameSpace = item.Namespace, NameClass = item.Name, Remark = null }).ToList();
            return implementTypes;
        }

        /// <summary>
        /// 立即执行任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task ExecuteJob(long jobId)
        {
            var model = await _tasksQzService.GetByIdAsync(jobId);
            if (model != null)
            {
                await _schedulerCenter.ExecuteJobAsync(model);
            }
        }
        /// <summary>
        /// 获取任务运行日志
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageData<TasksLog>> GetTaskLogs([FromQuery] Pagination pagination, int jobId, DateTime? runTimeStart = null, DateTime? runTimeEnd = null)
        {
            var model = await _tasksLogService.GetTaskLogs(pagination, jobId, runTimeStart, runTimeEnd);
            return model;
        }
        ///// <summary>
        ///// 任务概况
        ///// </summary>
        ///// <param name="jobId"></param>
        ///// <param name="page"></param>
        ///// <param name="pageSize"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<object> GetTaskOverview(int jobId, int page = 1, int pageSize = 10, DateTime? runTimeStart = null, DateTime? runTimeEnd = null, string type = "month")
        //{
        //    var model = await _tasksLogService.GetTaskOverview(jobId, runTimeStart, runTimeEnd, type);
        //    return model;
        //}
    }
}
