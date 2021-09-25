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
    [Authorize(Roles = "admin,editor")]
    [Area(areaName:"Admin")]
    public class PagesController : Controller
    {
        private readonly ShoppingAppContext _context;

        public PagesController(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in _context.Pages orderby p.Sorting select p;

            List<Page> pageList = await pages.ToListAsync();
            return View(pageList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == id);
            if (page == null) 
            {
                return NotFound();
            }

            return View(page);
        }


        public IActionResult Create() 
        {

            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Create(Page page) 
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;
                var slug = await _context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null) 
                {

                    ModelState.AddModelError("", "The title already exists");
                    return View(page);
                }
                _context.Add(page);
                await _context.SaveChangesAsync();

                TempData["success"] = "The page has been added!";
                return RedirectToAction("Index");
            }
            return View(page);

        
        }

        public async Task<IActionResult> Edit(int id)
        {
            var page = await _context.Pages.FindAsync( id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }


        [HttpPost]

        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug=page.Id==1 ? "home": page.Title.ToLower().Replace(" ", "-");
               
                var slug = await _context.Pages.Where(x=>x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {

                    ModelState.AddModelError("", "The page already exists");
                    return View(page);
                }
                _context.Update(page);
                await _context.SaveChangesAsync();

                TempData["success"] = "The page has been edited!";
                return RedirectToAction("Index");
            }
            return View(page);


        }
        //[HttpDelete]

        //public IActionResult Delete(int id) 
        //{
        //    var page = _context.Pages.FirstOrDefault(p => p.Id == id);

        //    if (page != null) 
        //    {

        //        _context.Pages.Remove(page);

        //        _context.SaveChanges();
        //        return Json(new { success = true, message = "The page deleted successfully" });
        //    }
        //    return Json(new { success = false, message = "Something went wrong" });

        //}


        public async Task<IActionResult> Delete(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "The page does not exists ";
            }
            else 
            {
                _context.Pages.Remove(page);
                await _context.SaveChangesAsync();
                TempData["success"] = "The page has been deleted ";
            }

            return RedirectToAction("Index");
        }
    }
}
