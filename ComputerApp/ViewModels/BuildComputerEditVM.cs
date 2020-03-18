using ComputerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.ViewModels
{
    public class BuildComputerEditVM
    {
        public List<CType> ComponentTypes { get; set; }
        public List<ComputerComponent> Components { get; set; }

        public BuildComputerEditVM(List<CType> componentTypes, List<ComputerComponent> components)
        {
            ComponentTypes = componentTypes;
            Components = components;
        }
    }
}
