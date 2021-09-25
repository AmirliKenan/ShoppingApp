using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;

namespace ShoppingApp.Infrastructure
{
    public class MainMenuVieWComponent:ViewComponent
    {
        private readonly ShoppingAppContext _context;

        public MainMenuVieWComponent(ShoppingAppContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync() 
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            return _context.Pages.OrderBy(p => p.Sorting).ToListAsync();
        }
    }
}
