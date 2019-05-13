using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuppliersProducts.Models
{
    public class Supplier
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Компания")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        public string FullName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Страна Происхождения")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s-]*$")]
        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Дата Поставки")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SupplyDate { get; set; }

        public ICollection<SupplierProduct> SupplierProduct { get; set; }
    }
}
