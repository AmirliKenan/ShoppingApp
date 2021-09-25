using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;

namespace ShoppingApp.Infrastructure
{
    public class CategoriesViewComponent:ViewComponent
    {
        private readonly ShoppingAppContext _context;

        public CategoriesViewComponent(ShoppingAppContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();
            return View(categories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return _context.Categories.OrderBy(p => p.Sorting).ToListAsync();
        }
    }
}
