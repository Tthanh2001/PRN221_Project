using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.PowerBI.Api.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Areas.Admin.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager= userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public List<AppUser> users { get; set; }
        public async Task OnGet()
        {
            users=await _userManager.Users.OrderBy(u => u.DisplayName).ToListAsync();
        }
        public void OnPost() => RedirectToPage();
    }
}
