using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViazyNetCore.Swagger
{
    public class SwaggerConfig
    {
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; } = false;
        /// <summary>
        /// 启用枚举架构过滤器
        /// </summary>
        public bool EnableEnumSchemaFilter { get; set; } = true;
        private string _RoutePrefix = "swagger";
        /// <summary>
        /// 访问地址
        /// </summary>
        public string RoutePrefix { get => Regex.Replace(_RoutePrefix, "^\\/+|\\/+$", ""); set => _RoutePrefix = value; }

        /// <summary>
        /// 地址
        /// </summary>
        public ApiServer[] ApiServer { get; set; } = Array.Empty<ApiServer>();

        /// <summary>
        /// 项目列表
        /// </summary>
        public List<ProjectConfig> Projects { get; set; } = new List<ProjectConfig>();
    }

    public class ApiServer
    {
        public string Scheme { get; set; }
        public HostString Host { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 项目配置
    /// </summary>
    public class ProjectConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
