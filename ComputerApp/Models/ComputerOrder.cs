using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Models
{
    public class ComputerOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ComputerId { get; set; }

        //Navigation Properties
        public Order Order { get; set; }
        public Computer Computer { get; set; }
    }
}
