using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth.Authorization.ViewModels
{
    public class MenusUpdateModel
    {
        public string? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类型 (0叶子节点，1中间节点， 2按钮)。
        /// </summary>
        public MenuType Type { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示URL地址。
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示子门户ID。
        /// </summary>
        public string? SysId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示排序。
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（-1删除，0禁用，1启用）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int OpenType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHomeShow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Exdata { get; set; }
    }
}
