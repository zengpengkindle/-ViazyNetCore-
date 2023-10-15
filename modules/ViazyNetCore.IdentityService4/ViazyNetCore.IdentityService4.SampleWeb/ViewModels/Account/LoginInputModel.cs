using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace ViazyNetCore.IdentityService4.SampleWeb.ViewModels
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [Description("用户名")]
        public string Username { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [Description("密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "验证码不能为空")]
        [Description("验证码")]
        public string Code { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }
}
