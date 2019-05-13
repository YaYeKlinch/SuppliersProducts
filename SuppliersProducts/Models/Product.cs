using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuppliersProducts.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="Название Продукта")]
        [RegularExpression(@"^[A-Z0-9]+[0-9a-zA-Z'\s-]*$")]
         public string Name { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Тип Продукта")]
        [RegularExpression(@"^[A-Z]+[a-z'\s-]*$")]
        public string Type { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<SupplierProduct> SuppliersProduct { get; set; }
    }
}
