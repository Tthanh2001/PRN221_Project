using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.PowerBI.Api.Models;
using PRN221_Project.Models;
using PRN221_Project.Utils;
using PRN221_Project.Pages.Admin.User;

namespace PRN221_Project.Pages.Admin.User
{
    [Authorize(Roles="Admin, Vip, Editor")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationAccount> _userManager;
        public IndexModel(UserManager<ApplicationAccount> userManager)
        {
            _userManager= userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public class UserAndRole : ApplicationAccount
        {
            public string RoleNames { get; set; }
        }
        public List<UserAndRole> users { get; set; }
        
        public async Task OnGet()
        {
            var qr = _userManager.Users.OrderBy(u => u.UserName);
            var qr1 = qr.Select(u => new UserAndRole()
            {
                Id= u.Id,
                UserName=u.UserName,

            });
            users=await qr1.ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames=string.Join(",", roles);
            }
        }
        public void OnPost() => RedirectToPage();
    }
}
