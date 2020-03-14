using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "This field can not be empty")]
        [MinLength(2)]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "This field can not be empty")]
        [Display(Name = "Date Of Birth")]
        public DateTime BirthDate { get; set; }

        //Nav Properties
        public Order Order { get; set; }
    }
}
