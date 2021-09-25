using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Models
{
    public class UserEdit
    {
      
        [Required,EmailAddress]

        public string Email { get; set; }
        [DataType(DataType.Password), MinLength(5, ErrorMessage = "Minimum length must be 5")]

        public string Password { get; set; }
        public UserEdit()
        {

        }
        public UserEdit(AppUser appUser)
        {
         
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
    }
}
