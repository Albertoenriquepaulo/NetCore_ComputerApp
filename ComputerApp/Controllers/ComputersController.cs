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

            DataForShoppingCartVM dataForShoppingCartVM = await _helperService.GetDataToSendToShoppingCartViewAsync();

            //Cantidad a imprimir en el carrito, usando Session, presente en HomeController, and LogOut
            int cantidad = await _orderService.GetHowManyComputerHasCurrentUserAsync(false);
            HttpContext.Session.SetString("SessionCartItemsNumber", JsonConvert.SerializeObject(cantidad));

            ViewData["myList"] = dataForShoppingCartVM.MyList;//myList;

            ViewData["totalPrice"] = await _helperService.GetTotalOrderPriceAsync(false);
            HttpContext.Session.SetString("SessionCartItemsTotalPrice", JsonConvert.SerializeObject(ViewData["totalPrice"]));
            HttpContext.Session.SetString("SessionCartItems", JsonConvert.SerializeObject(dataForShoppingCartVM.DataToSendToView));

            return View(dataForShoppingCartVM.DataToSendToView);
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

            Order orderAssociatedWUser = await _orderService.GetOrderItemAsync(false);

            //.ToListAsync();

            if (orderAssociatedWUser == null)
            {
                order.Price += _helperService.GetComputerTotalPrice(dataFromView);
                order.Qty = 1;
                order.IsCart = false;
                order.AppUserId = myCurrentUser.Id;
                orderId = await _helperService.InsertOrderToDBAsync(order);
            }
            else
            {
                orderId = orderAssociatedWUser.Id;
            }

            computer.Name = _helperService.CUSTOM_COMPUTER_NAME;
            computer.Price = _helperService.GetComputerTotalPrice(dataFromView);
            computer.IsDesktop = true;
            computer.ImgUrl = "https://c1.neweggimages.com/NeweggImage/ProductImage/83-221-575-V09.jpg";
            int computerId = await _helperService.InsertComputerToDBAsync(computer);
            int computerComponentId = await _helperService.InsertComponentsToComputerComponentDBAsync(dataFromView, computerId, orderId);
            int computerOrderId = await _helperService.InsertComputerOrderToDBAsync(orderId, computerId);

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

        //public async Task<IActionResult> CheckOut(int id, ComponentVM dataFromView)
        [HttpPost]
        public async Task<IActionResult> CheckOut(int id, ComponentVM dataFromView)
        {
            //bool exito = await _helperService.UpdateCheckOutFieldOfCurrentOrderAsync(true);
            await _helperService.DeleteOrderByCheckOutValueAsync(false);
            return RedirectToAction(nameof(Index), "Home");
        }

    }
}
