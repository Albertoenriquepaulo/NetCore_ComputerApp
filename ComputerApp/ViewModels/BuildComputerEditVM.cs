using ComputerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.ViewModels
{
    public class BuildComputerEditVM
    {
        public int ComputerId { get; set; }
        public List<CType> ComponentTypes { get; set; }
        public List<ComputerComponent> Components { get; set; }

        public BuildComputerEditVM(List<CType> componentTypes, List<ComputerComponent> components, int computerId)
        {
            ComponentTypes = componentTypes;
            Components = components;
            ComputerId = computerId;
        }
    }
}
