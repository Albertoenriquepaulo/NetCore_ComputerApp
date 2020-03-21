using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ComputerApp.Models;
using ComputerApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ComputerApp.Models.ShoppingCart;
using ComputerApp.Services;
using ComputerApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ComputerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly OrderService _orderService;
        private readonly HelperService _helperService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager, OrderService orderService, HelperService helperService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _orderService = orderService;
            _helperService = helperService;

        }

        public async Task<IActionResult> Index()
        {

            int cantidad = await _orderService.GetHowManyComputerHasCurrentUserAsync();
            //List<ComputerVM> cartDataFromControllers = new List<ComputerVM>();
            //cartDataFromControllers = JsonConvert.DeserializeObject<List<ComputerVM>>(HttpContext.Session.GetString("SessionCartItems"));
            HttpContext.Session.SetString("SessionCartItemsNumber", JsonConvert.SerializeObject(cantidad));

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Desktop(bool? isDesktop)
        {
            if (isDesktop == null)
            {
                return NotFound();
            }
            List<Computer> myComputers = new List<Computer>();
            // Para decirle a la vista que no ofrezca la opcion "Build your own Computer" cuando sea Laptop
            ViewData["isDesktop"] = isDesktop;
            myComputers = await _helperService.BuildComputerList((bool)isDesktop);

            return View(myComputers);
        }

        public async Task<ActionResult> AddToCart(int computerId)
        {
            List<Item> cart = new List<Item>();

            ComputerVM dataFromView = new ComputerVM();

            int orderId = 0;
            Order order = new Order();
            AppUser myCurrentUser = await _userManager.GetUserAsync(User);

            if (myCurrentUser == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Index));
            }

            Order orderAssociatedWUser = await _orderService.GetOrderItem();
            Computer computer = await _context.Computer.Include(computerOrderItem => computerOrderItem.ComputerOrders)
                                        .Where(computerItem => computerItem.Id == computerId)
                                        .FirstOrDefaultAsync();

            dataFromView = new ComputerVM
            {
                ComputerId = computerId,
                ImgUrl = computer.ImgUrl,
                Price = computer.Price,
                Qty = 1
            };

            if (orderAssociatedWUser == null)
            {
                //order.Price += _helperService.GetComputerTotalPrice(dataFromView);
                order.Price = computer.Price;
                order.Qty = 1;
                order.IsCart = true;
                order.AppUserId = myCurrentUser.Id;
                orderId = await _helperService.InsertOrderToDB(order);
            }
            else
            {
                orderId = orderAssociatedWUser.Id;
                order.Price = computer.Price;
                //TODO: Aqui debo actualizar el Precio de la Order
            }

            int computerOrderId = await _helperService.InsertComputerOrderToDB(orderId, computerId);

            //Updating CartItems
            int cantidad = await _orderService.GetHowManyComputerHasCurrentUserAsync();
            HttpContext.Session.SetString("SessionCartItemsNumber", JsonConvert.SerializeObject(cantidad));
            //FIN Updating CartItems

            return RedirectToAction(nameof(Desktop), new { isDesktop = computer.IsDesktop });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
