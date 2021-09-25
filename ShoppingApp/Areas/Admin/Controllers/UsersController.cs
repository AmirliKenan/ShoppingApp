using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models;

namespace ShoppingApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area(areaName:"Admin")]
    public class UsersController : Controller
    {
      
            private readonly UserManager<AppUser> _userManager;
          


            public UsersController(UserManager<AppUser> userManager)
          
            {
                _userManager = userManager;
            
            }

            public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
    }
}
