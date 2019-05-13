using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuppliersProducts.Models
{
    public class Order
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int BuyerID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Код Заказа")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s-]*$")]
        public string Place { get; set; }

        public Product Product { get; set; }
        public Buyer Buyer { get; set; }

    }
}
