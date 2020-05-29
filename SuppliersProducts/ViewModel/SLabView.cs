using Microsoft.AspNetCore.Http;
using SuppliersProducts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuppliersProducts.ViewModel
{
    public class SLabView
    {
        [Display(Name = "LabWork")]
        public int LabWorkID { get; set; }
        [Display(Name = "Student")]
        public int StudentID { get; set; }
        public IFormFile Path { get; set; }

        public LabWork LabWork { get; set; }
        public Student Student { get; set; }
    }
}
