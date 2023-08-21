using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PRN221_Project.Models;
using Microsoft.PowerBI.Api.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PRN221_Project.Areas.Admin.Pages.User
{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<ApplicationAccount> _userManager;
        private readonly SignInManager<ApplicationAccount> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AddRoleModel(UserManager<ApplicationAccount> userManager,
            SignInManager<ApplicationAccount> signInManager,
            RoleManager<IdentityRole> roleManager) { 
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        //
        [TempData]
        public string StatusMessage { get;set; }
        public ApplicationAccount user { get; set; }

        [BindProperty]
        [DisplayName("Roles assigned to user")]
       
        public string[] RoleNames { get; set; }
        public SelectList allRoles {  get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"User not found");
            }
            user=await _userManager.FindByIdAsync(id);
            if(user == null) {
                return NotFound($"User not found, id={id}.");
            }
            RoleNames=(await _userManager.GetRolesAsync(user)).ToArray<string>();
            //Danh sach de khoi tao selectlist
            List<string> roleNames =await _roleManager.Roles.Select(r=>r.Name)
                .ToListAsync();
            allRoles = new SelectList(roleNames);
            return Page();
        } 
        
        
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound($"User not found");
                
            }
            user=await _userManager.FindByIdAsync(id);
            if(user == null) 
            {
                return NotFound($"User not found, id={id}.");
            }
            //RoleNames
            var oldRoleName = (await _userManager.GetRolesAsync(user)).ToArray();
            var deleteRoles = oldRoleName.Where(r=>!RoleNames.Contains(r));
            var addRoles=RoleNames.Where(r=>!oldRoleName.Contains(r));

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name)
              .ToListAsync();
            allRoles = new SelectList(roleNames);

            var resultDelete=await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }
           

            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }

            StatusMessage = $"Updated role for user:{user.UserName}";
            return RedirectToPage("./Index");
        }
    }
}
