using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Utils;
using PRN221_Project.Pages.Admin.Role;

namespace PRN221_Project.Pages.Admin.Role
{
    //[Authorize(Roles ="Admin, Vip, Editor")]
    public class IndexModel : RolePageModel
    {
       
        public IndexModel(RoleManager<IdentityRole> roleManager,
            CinphileDbContext context) : base(roleManager,context)
        {

        }
        public List<IdentityRole> roles { get; set; }
        public async Task OnGet()
        {
            roles=await _roleManager.Roles.OrderBy(r=>r.Name).ToListAsync();
        }
        public void OnPost() => RedirectToPage();
    }
}
