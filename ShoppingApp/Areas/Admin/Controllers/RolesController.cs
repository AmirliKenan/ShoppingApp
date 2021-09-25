using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models;

namespace ShoppingApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> __userManager;

        public RolesController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            __userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([MinLength(2),Required] string name)
        {
            if (ModelState.IsValid) 
            {

                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    TempData["Success"] = "The role has been creted";
                    return RedirectToAction("Index");
                }
                else 
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                }

            }
            ModelState.AddModelError("", "Minimum length must be 2");
            return View();
        }

      
        public async Task<IActionResult> Edit( string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonmembers = new List<AppUser>();

            foreach (AppUser item in __userManager.Users)
            {
                var list = await __userManager.IsInRoleAsync(item, role.Name) ? members : nonmembers;
                list.Add(item);
            }

            return View(new RoleEdit 
            {
            Role=role,
            Members=members,
            NonMembers=nonmembers  
            });

        }

        [HttpPost]

        public async Task<IActionResult> Edit(RoleEdit roleEdit)
        {

            IdentityResult result;
            foreach (string userId in roleEdit.AddIds ?? new string[] { })
            {
                AppUser user = await __userManager.FindByIdAsync(userId);
                result = await __userManager.AddToRoleAsync(user,roleEdit.RoleName);

            }

            foreach (string userId in roleEdit.DeleteIds ?? new string[] { })
            {
                AppUser user = await __userManager.FindByIdAsync(userId);
                result = await __userManager.RemoveFromRoleAsync(user, roleEdit.RoleName);

            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
