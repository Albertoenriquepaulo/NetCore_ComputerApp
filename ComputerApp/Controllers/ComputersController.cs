using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComputerApp.Data;
using ComputerApp.Models;
using ComputerApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ComputerApp.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


//Video Roles y autorizaciones, usuario desde html min 20
namespace ComputerApp.Controllers
{
    [Authorize]
    public class ComputersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly OrderService _orderService;
        private readonly HelperService _helperService;

        public ComputersController(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, OrderService orderService, HelperService helperService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _orderService = orderService;
            _helperService = helperService;
        }

        // GET: Computers
        public async Task<IActionResult> Index()
        {
            List<Computer> myList = new List<Computer>();
            List<ComputerVM> dataToSendToView = new List<ComputerVM>();
            List<Component> ComponentList = await _context.Component.ToListAsync();
            AppUser myCurrentUser = await _userManager.GetUserAsync(User);

            //Obtengo la orden asociada al Usuario
            Order applicationDbContext = await _orderService.GetOrderItem();

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
                        await _helperService.UpdateComputerPrice(item);
                    }

                }
            }

            //Cantidad a imprimir en el carrito, usando Session, presente en HomeController, and LogOut
            int cantidad = await _orderService.GetHowManyComputerHasCurrentUserAsync();
            HttpContext.Session.SetString("SessionCartItemsNumber", JsonConvert.SerializeObject(cantidad));


            //Filtrando la lista, quitando sus elementos repetidos, ya que si hay un elemnto repetido
            //debo colocarlo en cantidad y no repetir elemento en la tabla de la vista
            myList = myList.GroupBy(computerItem => computerItem.Id)
                                                .Select(pc => pc.First())
                                                .ToList();
            ViewData["myList"] = myList;
            dataToSendToView = await _helperService.LoadComputerVM(myList, ComponentList);

            Order Order = await _orderService.GetOrderItem();


            if (Order != null) //Cuando el usuario ya ha introducido al menos una order en cart
            {
                ViewData["totalPrice"] = string.Format("€{0:N2}", Order.ComputerOrders.Select(co => co.Computer).Select(c => c.Price).Sum());

                HttpContext.Session.SetString("SessionCartItemsTotalPrice", JsonConvert.SerializeObject(ViewData["totalPrice"]));
            }
            else  ////Cuando el usuario es nuevo y NO ha introducido al menos una order en cart
            {
                ViewData["totalPrice"] = "0";
                HttpContext.Session.SetString("SessionCartItemsTotalPrice", JsonConvert.SerializeObject("0"));
            }

            HttpContext.Session.SetString("SessionCartItems", JsonConvert.SerializeObject(dataToSendToView));

            //JsonConvert.DeserializeObject<List<ComputerVM>>(HttpContext.Session.GetString("SessionCartItems"));

            return View(dataToSendToView);
        }

        // GET: Computers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computer = await _context.Computer
                .Include(c => c.ComputerOrders)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }

        // GET: Computers/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id");
            return View();
        }

        // POST: Computers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,IsDesktop,ImgUrl,OrderId")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", computer.OrderId);
            return View(computer);
        }

        // GET: Computers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computer = await _context.Computer.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }
            //ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", computer.OrderId);
            return View(computer);
        }

        // POST: Computers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,IsDesktop,ImgUrl,OrderId")] Computer computer)
        {
            if (id != computer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerExists(computer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", computer.OrderId);
            return View(computer);
        }

        // GET: Computers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computer = await _context.Computer
                //.Include(c => c.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }

        // POST: Computers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string nameOfCustomComputer = _helperService.CUSTOM_COMPUTER_NAME;//"Custom Computer";
            //var computer = await _context.Computer.FindAsync(id);
            var computer = await _context.Computer
                                .Where(computerItem => computerItem.Id == id)
                                .Include(computerOrderItem => computerOrderItem.ComputerOrders).FirstOrDefaultAsync();

            //El siguiente if nos asegura eliminar la Customm Computer de la tabla Computer
            //Pero si la pc a eliminar no es custom se debe eliminar la referencia de la tabla ComputerOrder
            if (computer.Name == nameOfCustomComputer)
            {
                _context.Computer.Remove(computer);

            }
            else
            {
                var computerOrder = await _context.ComputerOrder.FindAsync(computer.ComputerOrders[0].Id);
                _context.ComputerOrder.Remove(computerOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool ComputerExists(int id)
        {
            return _context.Computer.Any(e => e.Id == id);
        }

        //-----------------------------------------BUILD OWN COMPUTER
        // GET: Computers/BuildComputer/5
        public async Task<IActionResult> BuildComputer()
        {
            //Obtengo el objeto cuyo tipo es CPU, para con su ID obtener todas las posibles opciones de CPU
            List<CType> ComponentsList = await _context.CType.Include(c => c.Components).ToListAsync();

            return View(ComponentsList);
        }

        // POST: Computers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public void BuildComputer(string ProcessorId, string MemoryId, string Hdd, string Software)
        public async Task<IActionResult> BuildComputer(ComponentVM dataFromView)
        {
            Order order = new Order();
            Computer computer = new Computer();
            ComputerComponent computerComponent = new ComputerComponent();

            int orderId = 0;
            AppUser myCurrentUser = await _userManager.GetUserAsync(User);

            if (myCurrentUser == null)
            {
                //return NotFound();
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return RedirectToAction(nameof(Index));
            }

            Order orderAssociatedWUser = await _orderService.GetOrderItem();

            //.ToListAsync();

            if (orderAssociatedWUser == null)
            {
                order.Price += _helperService.GetComputerTotalPrice(dataFromView);
                order.Qty = 1;
                order.IsCart = false;
                order.AppUserId = myCurrentUser.Id;
                orderId = await _helperService.InsertOrderToDB(order);
            }
            else
            {
                orderId = orderAssociatedWUser.Id;
            }

            computer.Name = _helperService.CUSTOM_COMPUTER_NAME;
            computer.Price = _helperService.GetComputerTotalPrice(dataFromView);
            computer.IsDesktop = true;
            computer.ImgUrl = "https://c1.neweggimages.com/NeweggImage/ProductImage/83-221-575-V09.jpg";
            int computerId = await _helperService.InsertComputerToDB(computer);
            int computerComponentId = await _helperService.InsertComponentsToComputerComponentDB(dataFromView, computerId, orderId);
            int computerOrderId = await _helperService.InsertComputerOrderToDB(orderId, computerId);

            return RedirectToAction(nameof(Index));

        }

        // GET: Computers/BuildComputer/5
        public async Task<IActionResult> BuildComputerEdit(int? id)
        {
            List<ComputerComponent> components = new List<ComputerComponent>();

            //Obtengo todos los componentes que conforman el custom computer
            components = await _context.ComputerComponent
                                .Include(component => component.Component)
                                .Where(computer => computer.ComputerId == id)
                                .ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            //Obtengo todos los tipos de componentes, esto para crear los list box por cada tipo
            List<CType> componentTypes = await _context.CType.Include(c => c.Components).ToListAsync();

            if (componentTypes == null)
            {
                return NotFound();
            }

            return View(new BuildComputerEditVM(componentTypes, components, (int)id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public void BuildComputer(string ProcessorId, string MemoryId, string Hdd, string Software)
        public async Task<IActionResult> BuildComputerEdit(int id, ComponentVM dataFromView)
        {
            Type type = typeof(ComponentVM);
            int NumberOfRecords = type.GetProperties().Length;
            int[] idArray = new int[NumberOfRecords];

            Computer computer = await _context.Computer
                                        .Where(computer => computer.Id == id)
                                        .Include(component => component.ComputerComponents)
                                        .SingleOrDefaultAsync();
            if (id != computer.Id)
            {
                return NotFound();
            }
            computer.Price = _helperService.GetComputerTotalPrice(dataFromView);

            idArray[0] = dataFromView.HddId; idArray[1] = dataFromView.SoftwareId; idArray[2] = dataFromView.ProcessorId; idArray[3] = dataFromView.MemoryId; idArray[4] = dataFromView.OSId;
            int i = 0;
            foreach (var item in computer.ComputerComponents)
            {
                item.ComponentId = idArray[i++];
            }

            try
            {
                _context.Update(computer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComputerExists(computer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //-----------------------------------------END BUILD OWN COMPUTER
        //public double GetComputerTotalPrice(ComponentVM dataFromView)
        //{
        //    Type type = typeof(ComponentVM);
        //    int NumberOfRecords = type.GetProperties().Length;
        //    int[] idArray = new int[NumberOfRecords];
        //    double totalPrice = 0;
        //    Component component = new Component();
        //    idArray[0] = dataFromView.HddId; idArray[1] = dataFromView.SoftwareId; idArray[2] = dataFromView.ProcessorId; idArray[3] = dataFromView.MemoryId; idArray[4] = dataFromView.OSId;
        //    foreach (int item in idArray)
        //    {
        //        component = _context.Component.Where(c => c.Id == item).FirstOrDefault<Component>();
        //        totalPrice += component.Price;
        //    }
        //    return (totalPrice);
        //}

        //Inserta un elemento Computer nuevo en la bd Computer y devuelve el id de este elemento
        //public async Task<int> CreateFromCode([Bind("Id,Name,Price,IsDesktop,ImgUrl,OrderId")] Computer computer)
        //public async Task<int> InsertComputerToDB(Computer computer)
        //{
        //    _context.Add(computer);
        //    await _context.SaveChangesAsync();

        //    return computer.Id;
        //}
        //public async Task<int> InsertComponentsToComputerComponentDB(ComponentVM component, int computerId, int orderId)
        //{
        //    Type type = typeof(ComponentVM);
        //    int NumberOfRecords = type.GetProperties().Length;
        //    int[] idArray = new int[NumberOfRecords];

        //    ComputerComponent computerComponent = new ComputerComponent();
        //    idArray[0] = component.HddId; idArray[1] = component.SoftwareId; idArray[2] = component.ProcessorId; idArray[3] = component.MemoryId; idArray[4] = component.OSId;
        //    //computerComponent.ComputerId = computerId;
        //    foreach (var item in idArray)
        //    {
        //        computerComponent = new ComputerComponent();
        //        computerComponent.ComputerId = computerId;
        //        computerComponent.ComponentId = item;
        //        //computerComponent.OrderId = orderId;
        //        _context.Add(computerComponent);
        //        await _context.SaveChangesAsync();
        //    }

        //    return computerComponent.Id;
        //}

        ////Inserta un elemento Order nuevo en la bd Order y devuelve el id de este elemento
        //public async Task<int> InsertOrderToDB(Order order)
        //{
        //    _context.Add(order);
        //    await _context.SaveChangesAsync();

        //    return order.Id;
        //}

        ////Inserta un elemento ComputerOrder nuevo en la bd ComputerOrder y devuelve el id de este elemento
        //public async Task<int> InsertComputerOrderToDB(int orderId, int computerId)
        //{
        //    ComputerOrder computerOrder = new ComputerOrder();
        //    computerOrder.OrderId = orderId;
        //    computerOrder.ComputerId = computerId;
        //    _context.Add(computerOrder);
        //    await _context.SaveChangesAsync();

        //    return computerOrder.Id;
        //}
        //public async Task<int> InsertOrderToUser(Order order)
        //{
        //    _context.Add(order);
        //    await _context.SaveChangesAsync();
        //    return order.Id;
        //}

    }
}
