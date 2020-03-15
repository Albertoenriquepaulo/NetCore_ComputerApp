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

namespace ComputerApp.Controllers
{
    public class ComputersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public ComputersController(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Computers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Computer.Include(c => c.Order);

            /////////////////////
            AppUser myCurrentUser = await _userManager.GetUserAsync(User);
            string currentlyLoggedInUsername = User.Identity.Name;  //Por que puedo usar aqui el User, ¿¿¿donde está declarado???
            //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ////////////////////

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Computers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computer = await _context.Computer
                .Include(c => c.Order)
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
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", computer.OrderId);
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
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", computer.OrderId);
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
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", computer.OrderId);
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
                .Include(c => c.Order)
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
            var computer = await _context.Computer.FindAsync(id);
            _context.Computer.Remove(computer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputerExists(int id)
        {
            return _context.Computer.Any(e => e.Id == id);
        }

        //BUILD OWN COMPUTER
        public async Task<IActionResult> BuildComputer()
        {
            //Obtengo el objeto cuyo tipo es CPU, para con su ID obtener todas las posibles opciones de CPU
            List<CType> ComponentsList = await _context.CType.Include(c => c.Components).ToListAsync();


            //int idToSearch = _context.CType.Where(t => t.Name == "CPU").FirstOrDefault<CType>().Id;//.Include(c => c.ComponentType);

            //var appDbContextComponent = _context.Component.Include(c => c.ComponentType);
            //var prueba = appDbContextComponent.Where(t => t.ComponentType.Id == idToSearch);
            //Esta linea de abajo hace lo mismo que las dos linea de arriba
            //Obtengo todos los tipos de CPU que existen
            //var appDbContextComponent = _context.Component.Include(c => c.ComponentType).Where(t => t.ComponentType.Id == idToSearch);
            //List<Component> objectToSendToView = await appDbContextComponent.ToListAsync();

            //return View(objectToSendToView);
            return View(ComponentsList);
        }

        // POST: Computers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public void BuildComputer(string ProcessorId, string MemoryId, string Hdd, string Software)
        public async Task BuildComputer(ComponentVM dataFromView)
        {
            Computer computer = new Computer();
            Order order = new Order();

            order.Price += GetComputerTotalPrice(dataFromView);
            order.Qty = 1;
            order.IsCart = false;

            int orderId = await InsertOrderToDB(order);

            computer.Name = "Custom Computer";
            computer.Price = GetComputerTotalPrice(dataFromView);
            computer.IsDesktop = true;
            computer.OrderId = orderId; //TODO: Debo generar un order ID
            computer.ImgUrl = "https://c1.neweggimages.com/NeweggImage/ProductImage/83-221-575-V09.jpg";
            int computerId = await InsertComputerToDB(computer);
            int computerComponentId = await InsertComponentsToComputerComponentDB(dataFromView, computerId);



            //TODO: Debo de colocar computer en la tabla Computer para tener un ID y añadir los componentes
            //computer.ComputerComponents.Add();
            //ComputerComponentsController crear = new ComputerComponentsController(_context);


            //return View(ComponentsList);

        }
        //END BUILD OWN COMPUTER
        public double GetComputerTotalPrice(ComponentVM dataFromView)
        {
            int[] idArray = new int[4];
            double totalPrice = 0;
            Component component = new Component();
            idArray[0] = dataFromView.HddId; idArray[1] = dataFromView.SoftwareId; idArray[2] = dataFromView.ProcessorId; idArray[3] = dataFromView.MemoryId;
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
        public async Task<int> InsertComponentsToComputerComponentDB(ComponentVM component, int computerId)
        {
            int[] idArray = new int[4];
            ComputerComponent computerComponent = new ComputerComponent();
            idArray[0] = component.HddId; idArray[1] = component.SoftwareId; idArray[2] = component.ProcessorId; idArray[3] = component.MemoryId;
            //computerComponent.ComputerId = computerId;
            foreach (var item in idArray)
            {
                computerComponent = new ComputerComponent();
                computerComponent.ComputerId = computerId;
                computerComponent.ComponentId = item;
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
    }
}
