using ComputerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.ViewModels
{
    public class DataForShoppingCartVM
    {
        public List<ComputerVM> DataToSendToView { get; set; }
        public List<Computer> MyList { get; set; }

        public DataForShoppingCartVM(List<ComputerVM> dataToSendToView, List<Computer> myList)
        {
            DataToSendToView = dataToSendToView;
            MyList = myList;
        }
    }
}
