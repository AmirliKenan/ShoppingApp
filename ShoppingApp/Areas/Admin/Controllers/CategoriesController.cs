using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Infrastructure;
using ShoppingApp.Models;

namespace ShoppingApp.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin")]
    [Area(areaName:"Admin")]
    public class CategoriesController : Controller
    {
        private readonly ShoppingAppContext _context;

        public CategoriesController(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            
            return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Create() 
        {

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;
                var slug = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {

                    ModelState.AddModelError("", "The category already exists");
                    return View(category);
                }
                _context.Add(category);
                await _context.SaveChangesAsync();

                TempData["success"] = "The category has been added!";
                return RedirectToAction("Index");
            }
            return View(category);


        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id,Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug =category.Name.ToLower().Replace(" ", "-");

                var slug = await _context.Pages.Where(x => x.Id != category.Id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {

                    ModelState.AddModelError("", "The category already exists");
                    return View(category);
                }
                _context.Update(category);
                await _context.SaveChangesAsync();

                TempData["success"] = "The category has been edited!";
                return RedirectToAction("Index");
            }
            return View(category);


        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category does not exists ";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["success"] = "The category has been deleted ";
            }

            return RedirectToAction("Index");
        }
    }
}
