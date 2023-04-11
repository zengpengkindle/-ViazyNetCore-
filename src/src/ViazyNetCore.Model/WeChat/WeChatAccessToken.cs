using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViazyNetCore.Model
{
    public class WeChatAccessToken
    {
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [Column(IsPrimary = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int Id { get; set; }

        /// <summary>
        /// 类型1小程序2公众号
        /// </summary>
        [Display(Name = "类型1小程序2公众号")]
        [Required(ErrorMessage = "请输入{0}")]
        public int AppType { get; set; }

        /// <summary>
        /// appId
        /// </summary>
        [Display(Name = "appId")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string AppId { get; set; }

        /// <summary>
        /// accessToken
        /// </summary>
        [Display(Name = "accessToken")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(250, ErrorMessage = "{0}不能超过{1}字")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 第三方登录类型
        /// </summary>
        [Display(Name = "第三方登录类型")]
        public UserAccountTypes? Type { get; set; }

        /// <summary>
        ///     截止时间
        /// </summary>
        [Display(Name = "截止时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public long ExpireTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public long UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public long CreateTime { get; set; }
    }
}
