using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ViazyNetCore.Identity.AspNetCore.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = new IdentityUser()
            {
                Username = Request.Form["username"],
                Nickname = Request.Form["nickname"],
            };
            var identityResult = await this._userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                var newUser = await this._userManager.FindByNameAsync(user.Username);
                await this._userManager.AddPasswordAsync(newUser, Request.Form["password"]);
            }
            return Redirect("./Index");
        }
    }
}
