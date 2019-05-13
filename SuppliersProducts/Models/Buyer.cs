using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuppliersProducts.Models
{
    public class Buyer
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "ФИО")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Адресс")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s-]*$")]
        public string Address { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
