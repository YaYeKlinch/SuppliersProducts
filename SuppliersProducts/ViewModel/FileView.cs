using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuppliersProducts.ViewModel
{
    public class FileView
    {
        public int ID { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile file { get; set; }
    }
}
