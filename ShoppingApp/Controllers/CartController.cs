using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Infrastructure;
using ShoppingApp.Models;

namespace ShoppingApp.Controllers
{
    public class CartController : Controller
    {

        private readonly ShoppingAppContext _context;

        public CartController(ShoppingAppContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart")??new List<CartItem>();

            CartViewModel cartVM = new CartViewModel()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)

            };
            return View(cartVM);
        }
        public async Task<IActionResult> Add(int id)
        {
            Product product =await _context.Products.FindAsync(id);
           List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();
            if (cartItem == null)
            {
                cart.Add(new CartItem(product));

            }
            else 
            {
                cartItem.Quantity +=1;
            
            }
            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();
            if (cartItem.Quantity>1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);

            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else 
            {
                HttpContext.Session.SetJson("Cart", cart);

            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            
                cart.RemoveAll(x => x.ProductId == id);

            
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);

            }

            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {

          
                HttpContext.Session.Remove("Cart");
          

            return RedirectToAction("Index");
        }


    }
}
