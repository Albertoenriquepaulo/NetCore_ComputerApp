using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.ViewModels
{
    public class ComputerVM
    {
        public int ComputerId { get; set; }
        public string ImgUrl { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }

        //public double TotalPrice { get; set; }

        public double TotalPrice
        {
            get
            {
                return Qty * Price;
            }
        }

        public List<string> Products = new List<string>();
        //public List<string> Products { get; set; }

    }
}
