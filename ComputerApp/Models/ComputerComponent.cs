using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Models
{
    public class ComputerComponent
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public int ComponentId { get; set; }

        //Navigation Properties
        public Computer Computer { get; set; }
        public Component Component { get; set; }
    }
}
