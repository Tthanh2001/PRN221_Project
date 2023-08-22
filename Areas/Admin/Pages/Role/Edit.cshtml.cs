using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Project.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRN221_Project.Areas.Admin.Pages.Role
{
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, CinphileDbContext context) : base(roleManager, context)
        {
        }

        public class InputModel
        {
            [Display(Name ="Role name")]
            [Required(ErrorMessage ="Must input {0}")]
            [StringLength(256, MinimumLength =3, 
                ErrorMessage ="{0} must length {2} to {1} char")]
            public string Name { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public IdentityRole role {  get; set; }
        public async Task<IActionResult> OnGet(string roleid)
        {
            if (roleid == null)
            
                return NotFound("role not found");
            
            
             
            
            var role=await _roleManager.FindByIdAsync(roleid);
            if (role != null)
            {
                Input = new InputModel()
                {
                    Name=role.Name,
                };
                return Page();
            }
            return NotFound("role not found");
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("role not found");
            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("role not found");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            role.Name= Input.Name;
            var result=await _roleManager.UpdateAsync(role);

           
            if (result.Succeeded)
            {
                StatusMessage = $"role name changed:{Input.Name}";
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
