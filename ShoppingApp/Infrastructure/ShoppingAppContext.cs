using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Models;

namespace ShoppingApp.Infrastructure
{
    public class ShoppingAppContext:IdentityDbContext<AppUser>
    {
        public ShoppingAppContext(DbContextOptions<ShoppingAppContext> options):base(options)
        {
                
        }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
