using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ViazyNetCore.Model
{
    public class CmsUserWeChat
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [Column(IsPrimary = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public long Id { get; set; }
        /// <summary>
        /// 第三方登录类型
        /// </summary>
        [Display(Name = "第三方登录类型")]
        public UserAccountTypes? Type { get; set; }
        /// <summary>
        /// 关联用户表
        /// </summary>
        [Display(Name = "关联用户表")]
        [Required(ErrorMessage = "请输入{0}")]
        public int UserId { get; set; }
        /// <summary>
        /// openId
        /// </summary>
        [Display(Name = "openId")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Openid { get; set; }
        /// <summary>
        /// 缓存key
        /// </summary>
        [Display(Name = "缓存key")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string SessionKey { get; set; }
        /// <summary>
        /// unionid
        /// </summary>
        [Display(Name = "unionid")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string UnionId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Avatar { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        [Required(ErrorMessage = "请输入{0}")]
        public Gender Gender { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        [Display(Name = "语言")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Language { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市")]
        [StringLength(80, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string City { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        [Display(Name = "省")]
        [StringLength(80, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Province { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        [Display(Name = "国家")]
        [StringLength(80, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Country { get; set; }
        /// <summary>
        /// 手机号码国家编码
        /// </summary>
        [Display(Name = "手机号码国家编码")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string CountryCode { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public string Mobile { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
