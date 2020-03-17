using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Models
{
    public class Computer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "This field can not be empty")]
        [MinLength(2)]
        [Display(Name = "Name / Description")]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Is Desktop")]
        public bool IsDesktop { get; set; }

        [Display(Name = "URL Image")]
        public string ImgUrl { get; set; }

        //Navigation Properties
        public List<ComputerComponent> ComputerComponents { get; set; }

        //public int OrderId { get; set; }
        //public Order Order { get; set; }

        public List<ComputerOrder> ComputerOrders { get; set; }
    }
}
