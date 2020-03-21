using ComputerApp.Data;
using ComputerApp.Models;
using ComputerApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Services
{
    public class HelperService
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string CUSTOM_COMPUTER_NAME
        {
            get
            {
                return "Custom Computer";
            }
        }

        //public  MyProperty { get; set; }

        public HelperService(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public double GetComputerTotalPrice(ComponentVM dataFromView)
        {
            Type type = typeof(ComponentVM);
            int NumberOfRecords = type.GetProperties().Length;
            int[] idArray = new int[NumberOfRecords];
            double totalPrice = 0;
            Component component = new Component();
            idArray[0] = dataFromView.HddId; idArray[1] = dataFromView.SoftwareId; idArray[2] = dataFromView.ProcessorId; idArray[3] = dataFromView.MemoryId; idArray[4] = dataFromView.OSId;
            foreach (int item in idArray)
            {
                component = _context.Component.Where(c => c.Id == item).FirstOrDefault<Component>();
                totalPrice += component.Price;
            }
            return (totalPrice);
        }

        //Inserta un elemento Computer nuevo en la bd Computer y devuelve el id de este elemento
        //public async Task<int> CreateFromCode([Bind("Id,Name,Price,IsDesktop,ImgUrl,OrderId")] Computer computer)
        public async Task<int> InsertComputerToDB(Computer computer)
        {
            _context.Add(computer);
            await _context.SaveChangesAsync();

            return computer.Id;
        }
        public async Task<int> InsertComponentsToComputerComponentDB(ComponentVM component, int computerId, int orderId)
        {
            Type type = typeof(ComponentVM);
            int NumberOfRecords = type.GetProperties().Length;
            int[] idArray = new int[NumberOfRecords];

            ComputerComponent computerComponent = new ComputerComponent();
            idArray[0] = component.HddId; idArray[1] = component.SoftwareId; idArray[2] = component.ProcessorId; idArray[3] = component.MemoryId; idArray[4] = component.OSId;
            //computerComponent.ComputerId = computerId;
            foreach (var item in idArray)
            {
                computerComponent = new ComputerComponent();
                computerComponent.ComputerId = computerId;
                computerComponent.ComponentId = item;
                //computerComponent.OrderId = orderId;
                _context.Add(computerComponent);
                await _context.SaveChangesAsync();
            }

            return computerComponent.Id;
        }

        //Inserta un elemento Order nuevo en la bd Order y devuelve el id de este elemento
        public async Task<int> InsertOrderToDB(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        //Inserta un elemento ComputerOrder nuevo en la bd ComputerOrder y devuelve el id de este elemento
        public async Task<int> InsertComputerOrderToDB(int orderId, int computerId)
        {
            ComputerOrder computerOrder = new ComputerOrder();
            computerOrder.OrderId = orderId;
            computerOrder.ComputerId = computerId;
            _context.Add(computerOrder);
            await _context.SaveChangesAsync();

            return computerOrder.Id;
        }

        //Se actualiza el price en la tabla computer, recibe el objeto computer y actualiza el valor Price
        public async Task UpdateComputerPrice(Computer computer)
        {
            double price = 0;
            List<Component> Components = await _context.Component.ToListAsync();
            List<ComputerComponent> ComputerComponents = await _context.ComputerComponent.Where(computerItem => computerItem.ComputerId == computer.Id).ToListAsync();
            foreach (var computerComponent in ComputerComponents)
            {
                price += computerComponent.Component.Price;
            }
            computer.Price = price;
        }

        //Función que carga el objeto tipo ComputerVM para luego ser enviado a la vista
        public async Task<List<ComputerVM>> LoadComputerVM(List<Computer> myList, List<Component> ComponentList)
        {
            List<ComputerVM> dataToLoad = new List<ComputerVM>();
            foreach (Computer item in myList)
            {
                ComputerVM itemComputerVM = new ComputerVM();
                itemComputerVM.ComputerId = item.Id;
                itemComputerVM.ImgUrl = item.ImgUrl;
                itemComputerVM.Price = item.Price;
                itemComputerVM.Qty = await GetHowManyComputerWThisIDInComputerOrder(itemComputerVM.ComputerId);
                //itemComputerVM.TotalPrice = item.Price;
                foreach (ComputerComponent subItem in item.ComputerComponents)
                {
                    foreach (var componentItem in ComponentList)
                    {
                        if (componentItem.Id == subItem.ComponentId)
                        {
                            itemComputerVM.Products.Add(componentItem.Name);
                        }
                    }
                }
                dataToLoad.Add(itemComputerVM);
            }

            return (dataToLoad);
        }

        //Obtiene cuantas computadoras hay en ComputerOrder Dado el computerId, es decir del mismo modelo
        public async Task<int> GetHowManyComputerWThisIDInComputerOrder(int computerId)
        {
            List<ComputerOrder> ComputerOrders = await _context.ComputerOrder.Where(c => c.ComputerId == computerId).ToListAsync();
            return (ComputerOrders.Count());
        }


        // Contruye una lista de computadoras exeptuando la "Custom Computer" y si es Desktop or Laptop
        public async Task<List<Computer>> BuildComputerList(bool isDesktop)
        {
            List<Computer> computersFromContext = await _context.Computer.ToListAsync();
            List<Computer> Computers = computersFromContext
                                        .FindAll(computer => (computer.Name != CUSTOM_COMPUTER_NAME && computer.IsDesktop == isDesktop));

            return Computers;
        }

        public async Task<string> GetComputerNameAsync(int computerId)
        {
            Computer computer = await _context.Computer.Where(pc => pc.Id == computerId).FirstOrDefaultAsync();

            return computer.Name;
        }


    }
}
