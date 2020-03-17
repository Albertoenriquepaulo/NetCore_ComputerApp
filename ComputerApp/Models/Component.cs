using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Models
{
    public class Component
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "This field can not be empty")]
        [MinLength(2)]
        [Display(Name = "Component Descripcion")]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
        public double Price { get; set; }

        //Navigation Properties
        public int ComponentTypeId { get; set; }
        public CType ComponentType { get; set; }
        public List<ComputerComponent> ComputerComponents { get; set; }
    }
}
