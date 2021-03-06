﻿using System;
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
using Microsoft.AspNetCore.Authorization;

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
        private readonly GlobalValuesService _globalValuesService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager, OrderService orderService, HelperService helperService,
                                GlobalValuesService globalValuesService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _orderService = orderService;
            _helperService = helperService;
            _globalValuesService = globalValuesService;

        }

        public async Task<IActionResult> Index()
        {

            int cantidad = await _orderService.GetHowManyComputerHasCurrentUserAsync(false);

            //To Update ShoppingCartInfo

            DataForShoppingCartVM dataForShoppingCartVM = await _helperService.GetDataToSendToShoppingCartViewAsync();
            HttpContext.Session.SetString("SessionCartItems", JsonConvert.SerializeObject(dataForShoppingCartVM.DataToSendToView));

            ViewData["totalPrice"] = await _helperService.GetTotalOrderPriceAsync(false);
            HttpContext.Session.SetString("SessionCartItemsTotalPrice", JsonConvert.SerializeObject(ViewData["totalPrice"]));
            //FIN To Update ShoppingCartInfo

            HttpContext.Session.SetString("SessionCartItemsNumber", JsonConvert.SerializeObject(cantidad));

            ViewData["CTypes"] = await _context.CType.ToListAsync();
            ViewData["ComputerComponents"] = await _context.ComputerComponent.Include(c => c.Component).Include(c => c.Computer).ToListAsync();
            ViewData["Components"] = await _context.Component.Include(c => c.ComponentType).ToListAsync();
            //ViewData["Computers"] = await _context
            //ViewData["Orders"] = await _context
            ViewData["MyUsers"] = await _userManager.Users.ToListAsync();

            ViewBag.Message = $"The user has been deleted";

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
            myComputers = await _helperService.BuildComputerListAsync((bool)isDesktop);

            //To Update ShoppingCartInfo
            DataForShoppingCartVM dataForShoppingCartVM = await _helperService.GetDataToSendToShoppingCartViewAsync();
            HttpContext.Session.SetString("SessionCartItems", JsonConvert.SerializeObject(dataForShoppingCartVM.DataToSendToView));

            ViewData["totalPrice"] = await _helperService.GetTotalOrderPriceAsync(false);
            HttpContext.Session.SetString("SessionCartItemsTotalPrice", JsonConvert.SerializeObject(ViewData["totalPrice"]));
            //FIN To Update ShoppingCartInfo

            return View(myComputers);
        }

        [Authorize]
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

            Order orderAssociatedWUser = await _orderService.GetOrderItemAsync(false);
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

            //Cuando el usuario no tiene una order, se crea una nueva, con (order.IsCart = true) y (order.CheckOut) = false;
            if (orderAssociatedWUser == null || orderAssociatedWUser.Id == 0)
            {
                order.Price = computer.Price;
                order.Qty = 1;
                order.IsCart = true;
                order.CheckOut = false;
                order.AppUserId = myCurrentUser.Id;
                orderId = await _helperService.InsertOrderToDBAsync(order);
            }
            else
            {
                orderId = orderAssociatedWUser.Id;
                order.Price = computer.Price;
                //TODO: Aqui debo actualizar el Precio de la Order
            }

            int computerOrderId = await _helperService.InsertComputerOrderToDBAsync(orderId, computerId);

            //Updating CartItems
            int cantidad = await _orderService.GetHowManyComputerHasCurrentUserAsync(false);
            HttpContext.Session.SetString("SessionCartItemsNumber", JsonConvert.SerializeObject(cantidad));
            //FIN Updating CartItems

            return RedirectToAction(nameof(Desktop), new { isDesktop = computer.IsDesktop });
        }


        /// <summary>
        /// Aqui me quede 23/03
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditUser(string id)
        {
            AppUser myUser = _userManager.Users.Include(o => o.Order).First(u => u.Id == id);
            return View(myUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, string selectedRole)
        {
            AppUser myUser = _userManager.Users.Include(o => o.Order).First(u => u.Id == id);
            IEnumerable<string> roles = await _helperService.GetUserRoleAsync(myUser);

            if (_helperService.RolExistAsync(roles, selectedRole))
            {
                await _helperService.RemoveRolesAsync(myUser, roles);
                await _helperService.AddRolAsync(myUser, selectedRole);
            }
            else
            {
                await _helperService.AddRolAsync(myUser, selectedRole);
            }

            _globalValuesService.SetShowMessage(true);
            _globalValuesService.SetMessage($"Rol of the user '{myUser.Name.ToUpper()}' has been updated to '{selectedRole.ToUpper()}'");
            return View(myUser);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            AppUser myUser = await _userManager.FindByIdAsync(id);
            string msg = null;

            if (myUser == null)
            {
                msg = $"User with Id = {id} cannot be found";
            }
            else if (myUser.Name == "root")
            {
                msg = "The user 'root' can not be deleted";
            }
            else if (myUser.Email == User.Identity.Name)
            {
                msg = "You can not delete yourselve...";
            }
            else
            {
                await _helperService.DeleteOrderByUserIdAsync(id);
                var result = await _userManager.DeleteAsync(myUser);

                if (result.Succeeded)
                {
                    msg = $"The user '{myUser.Name}' has been removed from the Database";
                }
                else
                {
                    msg = $"An unknown error ocurred. Contact the developer of this application...";
                }
            }
            _globalValuesService.SetShowMessage(true);
            _globalValuesService.SetMessage(msg);
            return LocalRedirect("~/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
