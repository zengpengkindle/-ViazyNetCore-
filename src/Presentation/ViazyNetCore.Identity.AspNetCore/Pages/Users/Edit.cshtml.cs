using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViazyNetCore.Dtos;

namespace ViazyNetCore.Identity.AspNetCore.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public long Id { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }


        public EditModel(UserManager<IdentityUser> userManager, IMapper mapper)
        {
            this._userManager = userManager;
            this._mapper = mapper;
        }

        public async Task<IActionResult> OnGet(long id)
        {
            var user = await this._userManager.FindByIdAsync(id.ToString());
            Id = id;
            Name = user.Username;
            NickName = user.Nickname;

            return Page();
        }

        public async Task<IActionResult> OnPost([FromQuery] long id, string nickName)
        {
            var user = await this._userManager.FindByIdAsync(id.ToString());
            user.Nickname = nickName;

            await this._userManager.UpdateAsync(user);
            return Redirect("./Index");
        }
    }
}
