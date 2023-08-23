using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Admin.Role
{
    public class RolePageModel:PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly CinphileDbContext _context;
        [TempData]
        public string StatusMessage { get;set; }
        public RolePageModel(RoleManager<IdentityRole> roleManager, CinphileDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
    }
}
