using ComputerApp.Data;
using ComputerApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    }
}
