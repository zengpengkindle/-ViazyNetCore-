﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TaskScheduler
{
    /// <summary>
    /// 服务调度接口
    /// </summary>
    public interface ISchedulerCenter
    {
        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        Task<bool> StartScheduleAsync();
        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        Task<bool> StopScheduleAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task AddScheduleJobAsync(TaskInfo sysSchedule);
        /// <summary>
        /// 停止一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task StopScheduleJobAsync(TaskInfo sysSchedule);
        /// <summary>
        /// 检测任务是否存在
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<bool> IsExistScheduleJobAsync(TaskInfo sysSchedule);
        /// <summary>
        /// 暂停指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task PauseJob(TaskInfo sysSchedule);
        /// <summary>
        /// 恢复一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task ResumeJob(TaskInfo sysSchedule);

        /// <summary>
        /// 获取任务触发器状态
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<List<TaskInfoDto>> GetTaskStaus(TaskInfo sysSchedule);
        /// <summary>
        /// 获取触发器标识
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetTriggerState(string key);

        /// <summary>
        /// 立即执行 一个任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        Task ExecuteJobAsync(TaskInfo tasksQz);

        /// <summary>
        /// 停止一个指定的Trigger
        /// </summary>
        Task PauseScheduleTriggerAsync(TaskInfo model, string triggerId);
        Task ResumeScheduleTriggerAsync(TaskInfo model, string triggerId);
    }
}
