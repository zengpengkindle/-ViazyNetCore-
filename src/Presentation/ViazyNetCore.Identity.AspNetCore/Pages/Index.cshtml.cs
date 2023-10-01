using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Identity.Domain;

namespace ViazyNetCore.Identity.AspNetCore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<Domain.IdentityUser> _userManager;
        private readonly SignInManager<Domain.IdentityUser> _signInManager;

        public IndexModel(ILogger<IndexModel> logger
            , UserManager<Domain.IdentityUser> userManager
            , SignInManager<Domain.IdentityUser> signInManager)
        {
            _logger = logger;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            var username = this.Request.Form["username"];
            var password = this.Request.Form["password"];

            var user = await this._userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new ApiException("用户不存在");
            }

            var signInResult = await this._signInManager.CheckPasswordSignInAsync(user, password, true);
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    throw new ApiException($"用户因密码错误次数过多而被锁定 {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} 分钟，请稍后重试");
                }
                if (signInResult.IsNotAllowed)
                {
                    throw new ApiException("不允许登录。");
                }
                throw new ApiException("登录失败，用户名或账号无效。");
            }
            return Redirect("Privacy");
        }
    }
}