using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        //public Computer Computer { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }

        [Required]
        [Display(Name = "Is in the Cart")]
        public bool IsCart { get; set; }
        public bool CheckOut { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public List<Computer> Computers { get; set; }
    }
}
