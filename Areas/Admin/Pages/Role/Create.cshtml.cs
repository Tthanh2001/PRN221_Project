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
    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, CinphileDbContext context) : base(roleManager, context)
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
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var newRole=new IdentityRole(Input.Name);
            var result=await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"New role created:{Input.Name}";
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
