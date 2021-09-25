using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Models
{
    public class User
    {
        [Required,MinLength(2,ErrorMessage ="Minimum length must be 2")]
        public string UserName { get; set; }
        [Required,EmailAddress]

        public string Email { get; set; }
        [DataType(DataType.Password),Required, MinLength(5, ErrorMessage = "Minimum length must be 5")]

        public string Password { get; set; }
        
    }
}
