using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Model;

namespace ViazyNetCore.Manage.WebApi.ViewModel
{
    public class MenuPageItem
    {
        /// <summary>
        /// 获取或设置一个值，表示是否为分组。
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示页面标题。
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// 获取或设置一个值，表示语言包。
        /// </summary>
        public Dictionary<string, string>? LangTitles { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示页面图标。
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示页面路径。
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示状态（0禁用，1启用，-1删除）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示子页面列表。
        /// </summary>
        public List<MenuPageItem>? Children { get; set; }
    }
}
