using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViazyNetCore.Dtos;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Identity.AspNetCore.Pages.Users;

public class IndexModel : PageModel
{
    private readonly IUserService _userService;

    public IndexModel(IUserService userService)
    {
        this._userService = userService;
    }

    [BindProperty]
    public PageData<UserDto> PageData { get; set; }

    [BindProperty]
    public int PageIndex { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        if (Request.Query["page"].IsNullOrEmpty())
        {
            PageIndex = 1;
        }
        else
        {
            PageIndex = Request.Query["page"].First().ParseTo<int>();
        }
        PageData = await _userService.FindAllAsync(new Dtos.UserFindQueryDto
        {
            Page = PageIndex,
            Limit = 10
        });

        return Page();
    }
}
