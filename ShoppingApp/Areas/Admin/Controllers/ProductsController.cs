using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Infrastructure;
using ShoppingApp.Models;

namespace ShoppingApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area(areaName:"Admin")]
    public class ProductsController : Controller
    {
        private readonly ShoppingAppContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ShoppingAppContext context, IWebHostEnvironment webHostEnvironment)
        { 
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.OrderByDescending(x=>x.Id).Include(x=>x.Category).ToListAsync());
        }


        public IActionResult Create() 
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                var slug = await _context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {

                    ModelState.AddModelError("", "The product already exists");
                    return View(product);
                }
                string imageName = "noimage.png";
                if (product.ImageUpload != null) 
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                
                }
                product.Image = imageName;
                _context.Add(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "The product has been added!";
                return RedirectToAction("Index");
            }
            return View(product);


        }


        public async Task<IActionResult> Details(int id)
        {
            Product product = await _context.Products.Include(x=>x.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId);
            return View(product);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id,Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                var slug = await _context.Products.Where(x=>x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {

                    ModelState.AddModelError("", "The product already exists");
                    return View(product);
                }
               
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

                    if (!string.Equals(product.Image, "noimage.png"))
                    
                    {

                    string oldImagePath = Path.Combine(uploadsDir, product.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }
                 string   imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }
             
                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "The product has been edited!";
                return RedirectToAction("Index");
            }
            return View(product);


        }

        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "The product does not exists ";
            }
            else
            {

                if (!string.Equals(product.Image, "noimage.png"))

                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

                    string oldImagePath = Path.Combine(uploadsDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["success"] = "The product has been deleted ";
            }

            return RedirectToAction("Index");
        }

    }
}
