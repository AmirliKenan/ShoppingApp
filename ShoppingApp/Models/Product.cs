using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ShoppingApp.Infrastructure;

namespace ShoppingApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "minimum length is 2")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "minimum length is 4")]
        public string Description { get; set; }
        [Column(TypeName ="decimal(18,2)")]  
        public decimal Price { get; set; }
        [Display(Name ="Category")]
        [Range(1,int.MaxValue,ErrorMessage ="You must choose a category")]
        public int CategoryId { get; set; }
       
        public string Image { get; set; }
        [ForeignKey("CategoryId")]
        [Range(1,int.MaxValue,ErrorMessage ="You must choose category")]
        public virtual Category Category { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
