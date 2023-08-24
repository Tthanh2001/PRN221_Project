using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Utils;
using PRN221_Project.Pages.Admin.Role;
using PRN221_Project.Models;

namespace PRN221_Project.Pages.Admin.Role
{
    [Authorize(Roles = "Admin, Vip, Editor")]
    public class IndexModel : RolePageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(RoleManager<IdentityRole> roleManager,
            CinphileDbContext context) : base(roleManager, context)
        {
            _roleManager = roleManager;
        }
        public List<IdentityRole> roles { get; set; }
        public async Task OnGet()
        {
            roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
        }
        public void OnPost() => RedirectToPage();
    }
}
