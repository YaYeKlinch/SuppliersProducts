using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuppliersProducts.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Not set Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Not set Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
