﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.TaskScheduler
{
    public class TaskInfo:EntityUpdate
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [Column(StringLength = 200, IsNullable = true)]
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        [Column(StringLength = 200, IsNullable = true)]
        public string JobGroup { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        [Column(StringLength = 200, IsNullable = true)]
        public string Cron { get; set; }
        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [Column(StringLength = 200, IsNullable = true)]
        public string AssemblyName { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        [Column(StringLength = 200, IsNullable = true)]
        public string ClassName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [Column(StringLength = -2, IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 并行触发数
        /// </summary>
        public int TriggerCount { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        public int RunTimes { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 触发器类型（0、simple 1、cron）
        /// </summary>
        public int TriggerType { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 循环执行次数
        /// </summary>
        public int CycleRunTimes { get; set; }
        /// <summary>
        /// 已循环次数
        /// </summary>
        public int CycleHasRunTimes { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get; set; } = false;
        /// <summary>
        /// 执行传参
        /// </summary>
        public string JobParams { get; set; }


        [Column(IsNullable = true)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 任务内存中的状态
        /// </summary>
        [Column(IsIgnore = true)]
        public List<TaskInfoDto> Triggers { get; set; }
    }
}
