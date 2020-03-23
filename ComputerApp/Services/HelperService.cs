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
        private readonly OrderService _orderService;
        public string CUSTOM_COMPUTER_NAME
        {
            get
            {
                return "Custom Computer";
            }
        }

        //public  MyProperty { get; set; }

        public HelperService(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor, OrderService orderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
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
        public async Task<int> InsertComputerToDBAsync(Computer computer)
        {
            _context.Add(computer);
            await _context.SaveChangesAsync();

            return computer.Id;
        }
        public async Task<int> InsertComponentsToComputerComponentDBAsync(ComponentVM component, int computerId, int orderId)
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
        public async Task<int> InsertOrderToDBAsync(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        //Inserta un elemento ComputerOrder nuevo en la bd ComputerOrder y devuelve el id de este elemento
        public async Task<int> InsertComputerOrderToDBAsync(int orderId, int computerId)
        {
            ComputerOrder computerOrder = new ComputerOrder();
            computerOrder.OrderId = orderId;
            computerOrder.ComputerId = computerId;
            _context.Add(computerOrder);
            await _context.SaveChangesAsync();

            return computerOrder.Id;
        }

        //Se actualiza el price en la tabla computer, recibe el objeto computer y actualiza el valor Price
        public async Task UpdateComputerPriceAsync(Computer computer)
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
        public async Task<List<ComputerVM>> LoadComputerVMAsync(List<Computer> myList, List<Component> ComponentList)
        {
            List<ComputerVM> dataToLoad = new List<ComputerVM>();
            foreach (Computer item in myList)
            {
                ComputerVM itemComputerVM = new ComputerVM();
                itemComputerVM.ComputerId = item.Id;
                itemComputerVM.ImgUrl = item.ImgUrl;
                itemComputerVM.Price = item.Price;
                itemComputerVM.Qty = await GetHowManyComputerWThisIDInComputerOrderAsync(itemComputerVM.ComputerId);
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
        public async Task<int> GetHowManyComputerWThisIDInComputerOrderAsync(int computerId)
        {
            List<ComputerOrder> ComputerOrders = await _context.ComputerOrder.Where(c => c.ComputerId == computerId).ToListAsync();
            return (ComputerOrders.Count());
        }


        // Contruye una lista de computadoras exceptuando la "Custom Computer" y si es Desktop or Laptop
        public async Task<List<Computer>> BuildComputerListAsync(bool isDesktop)
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

        //Obtiene la data para ser enviada al Shopping Cart view que es el Index del ComputersCorntroller
        public async Task<DataForShoppingCartVM> GetDataToSendToShoppingCartViewAsync()
        {
            List<Computer> myList = new List<Computer>();
            List<ComputerVM> dataToSendToView = new List<ComputerVM>();
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                List<Component> ComponentList = await _context.Component.ToListAsync();


                Order applicationDbContext = await _orderService.GetOrderItemAsync(false);

                if (applicationDbContext != null)
                {
                    foreach (var item in applicationDbContext.ComputerOrders)
                    {
                        myList.Add(item.Computer);
                    }
                    foreach (Computer item in myList)
                    {
                        if (item.ComputerComponents.Count > 0) //Solo actualizará precio cuando el computer sea Custom
                        {
                            await UpdateComputerPriceAsync(item);
                        }

                    }
                }
                //Filtrando la lista, quitando sus elementos repetidos, ya que si hay un elemnto repetido
                //debo colocarlo en cantidad y no repetir elemento en la tabla de la vista
                myList = myList.GroupBy(computerItem => computerItem.Id)
                                                    .Select(pc => pc.First())
                                                    .ToList();

                dataToSendToView = await LoadComputerVMAsync(myList, ComponentList);

                DataForShoppingCartVM dataForShoppingCartVM = new DataForShoppingCartVM(dataToSendToView, myList);

                return dataForShoppingCartVM;
            }

            return (new DataForShoppingCartVM(dataToSendToView, myList));
        }

        public async Task<string> GetTotalOrderPriceAsync(bool checkOut)
        {
            double totalPrice = 0;

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                Order Order = await _orderService.GetOrderItemAsync(checkOut);

                if (Order != null) //Cuando el usuario ya ha introducido al menos una order en cart
                {
                    totalPrice = Order.ComputerOrders.Select(co => co.Computer).Select(c => c.Price).Sum();
                }
            }

            return string.Format("€{0:N2}", totalPrice);
        }

        //Actualiza el campo CheckOut de la Order, devuelve true si es exitoso y false si no lo es 
        //Lo setea al valor recibido en checkOut
        public async Task<bool> UpdateCheckOutFieldOfCurrentOrderAsync(bool checkOut)
        {
            Order order = await _orderService.GetOrderItemAsync(!checkOut); //Buscamos el order que debe estar en el contrario de checkOut
            if (order != null)
            {
                order.CheckOut = checkOut;
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public async Task DeleteOrderAsync(bool checkOut)
        {
            Order order = await _orderService.GetOrderItemAsync(checkOut); //Buscamos el order que debe estar en el contrario de checkOut
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
