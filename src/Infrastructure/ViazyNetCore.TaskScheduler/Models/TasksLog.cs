using FreeSql.DataAnnotations;
using ViazyNetCore;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 任务日志表
    /// </summary>
    public class TaskLog : EntityAdd
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public long JobId { get; set; }
        /// <summary>
        /// 任务耗时
        /// </summary>
        public double TotalTime { get; set; }
        /// <summary>
        /// 执行结果(0-失败 1-成功)
        /// </summary>
        public bool RunResult { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        public DateTime RunTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 执行参数
        /// </summary>
        [Column(StringLength = 500, IsNullable = true)]
        public string? RunPars { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        [Column(StringLength = 500, IsNullable = true)]
        public string? ErrMessage { get; set; }
        /// <summary>
        /// 异常堆栈
        /// </summary>
        [Column(StringLength = 2000, IsNullable = true)]
        public string? ErrStackTrace { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        [Column(IsIgnore = true)]
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        [Column(IsIgnore = true)]
        public string JobGroup { get; set; }
    }
}
