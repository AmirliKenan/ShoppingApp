using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Infrastructure;
using ShoppingApp.Models;

namespace ShoppingApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ShoppingAppContext _context;

        public ProductsController(ShoppingAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.OrderByDescending(x => x.Id).ToListAsync());
        }

        public async Task<IActionResult> ProductsByCategory(string categorySlug)
        {
            Category category =await _context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");
            var products = _context.Products.Where(x => x.CategoryId == category.Id);
            ViewBag.CategoryName = category.Name;
            return View(await products.ToListAsync());
        }
    }
}
