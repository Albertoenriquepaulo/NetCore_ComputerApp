using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.ViewModels
{
    public class ComponentVM
    {
        public int ProcessorId { get; set; }
        public int MemoryId { get; set; }
        public int HddId { get; set; }
        public int SoftwareId { get; set; }
        public int OSId { get; set; }

        //public double Total
        //{
        //    get
        //    {
        //        return typeof(ComponentVM)
        //            .GetProperties()
        //            .Where(x => x.PropertyType == typeof(double)
        //               && x.Name != "Total")
        //            .Sum(p => (double)p.GetValue(this));
        //    }
        //}
    }
}
