using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ShoppingApp.Models;

namespace ShoppingApp.Infrastructure
{
    [HtmlTargetElement("td",Attributes ="user-role")]
    public class RolesTagHelper:TagHelper
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> __userManager;

        public RolesTagHelper(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            __userManager = userManager;
        }
        [HtmlAttributeName("user-role")]
        public string RoleId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) 
        {
            List<string> names = new List<string>();
            IdentityRole role = await _roleManager.FindByIdAsync(RoleId);
            if (role != null) 
            {
                foreach (var item in __userManager.Users)
                {
                    if (item != null && await __userManager.IsInRoleAsync(item, role.Name)) 
                    {
                        names.Add(item.UserName);
                    }

                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
        
        }
    }
}
