using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using ShoppingApp.Infrastructure;
using ShoppingApp.Models;

namespace ShoppingApp.Controllers
{
    public class PagesController : Controller
    {
        private readonly ShoppingAppContext _context;

        public PagesController(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null) 
            {
                return View(await _context.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }
            Page page = await _context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();
            if (page == null) 
            {
                return NotFound();
            }
            return View(page);
        }
    }
}
