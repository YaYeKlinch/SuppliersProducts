using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuppliersProducts.Models
{
    public class SupplierProduct
    {
        public int ID { get; set; }

        [Display(Name = "Продукт")]
        public int ProductID { get; set; }
        [Display(Name = "Поставщик")]
        public int SupplierID { get; set; }

        public Product Product { get; set; }
        public Supplier Supplier { get; set; }

    }
}
