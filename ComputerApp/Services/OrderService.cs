using ComputerApp.Data;
using ComputerApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ComputerApp.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Order> GetOrderItem()
        {
            AppUser myCurrentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            Order order = await _context.Order
                .Where(orderItem => orderItem.AppUserId == myCurrentUser.Id)
                .Include(pcOrderItem => pcOrderItem.ComputerOrders)
                    .ThenInclude(orderItem => orderItem.Order)
                .Include(pcOrderItem => pcOrderItem.ComputerOrders)
                    .ThenInclude(pcItem => pcItem.Computer)
                        .ThenInclude(pcComponentItem => pcComponentItem.ComputerComponents)
                .SingleOrDefaultAsync();

            return order;
        }
        //Obtiene cuantas computadoras hay en ComputerOrder Dado el userId
        public async Task<int> GetHowManyComputerHasCurrentUserAsync()
        {
            AppUser myCurrentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            Order order = new Order();
            order.ComputerOrders = new List<ComputerOrder>();
            if (myCurrentUser != null)
            {
                order = await _context.Order.Where(o => o.AppUserId == myCurrentUser.Id)
                                              .Include(co => co.ComputerOrders)
                                              .FirstOrDefaultAsync();
                if (order == null)
                {
                    return 0;
                }
            }
            return (order.ComputerOrders.Count());
        }

        //public async Task SetSessionCartItems(int? cantidad)
        //{
        //    AppUser myCurrentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        //    if (myCurrentUser != null)
        //    {
        //        if (cantidad == null)
        //        {
        //            cantidad = await GetHowManyComputerHasCurrentUserAsync();
        //        }
        //    }

        //    HttpContext.Session.SetString("userCartItems", JsonConvert.SerializeObject(cantidad));

        //}

    }
}
