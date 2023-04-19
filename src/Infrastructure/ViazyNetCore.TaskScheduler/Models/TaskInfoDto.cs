using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TaskScheduler
{
    public class TaskInfoDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string JobId { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        public string JobGroup { get; set; }
        /// <summary>
        /// 触发器ID
        /// </summary>
        public string TriggerId { get; set; }
        /// <summary>
        /// 触发器名称
        /// </summary>
        public string TriggerName { get; set; }
        /// <summary>
        /// 触发器分组
        /// </summary>
        public string TriggerGroup { get; set; }
        /// <summary>
        /// 触发器状态
        /// </summary>
        public string TriggerStatus { get; set; }
    }
}
