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

namespace ComputerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
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
            myComputers = await BuildComputerList((bool)isDesktop);
            return View(myComputers);
        }

        // Contruye una lista de computadoras exeptuando la "Custom Computer" y si es Desktop or Laptop
        public async Task<List<Computer>> BuildComputerList(bool isDesktop)
        {
            List<Computer> computersFromContext = await _context.Computer.ToListAsync();
            List<Computer> Computers = new List<Computer>();
            List<string> ComputerNames = await _context.Computer.Select(m => m.Name).Distinct().ToListAsync();
            ComputerNames.RemoveAll(u => u.StartsWith("Custom"));

            foreach (Computer computerItem in computersFromContext)
            {
                if (ComputerNames.Contains(computerItem.Name) && computerItem.IsDesktop == isDesktop)
                {
                    Computers.Add(computerItem);
                }
            }
            return Computers;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
