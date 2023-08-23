using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Project.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PRN221_Project.Pages.Admin.Role;

namespace PRN221_Project.Pages.Admin.Role
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, CinphileDbContext context) : base(roleManager, context)
        {
        }

      
        public IdentityRole role {  get; set; }
        public async Task<IActionResult> OnGet(string roleid)
        {
            if (roleid == null)
            
                return NotFound("role not found");
            
            var role=await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("role not found");
            }
           return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("role not found");
            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("role not found");

            var result = await _roleManager.DeleteAsync(role);

           
            if (result.Succeeded)
            {
                StatusMessage = $"You have deleted:{role.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            
            return Page();
        }
    }
}
