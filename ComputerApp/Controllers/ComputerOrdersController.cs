using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComputerApp.Data;
using ComputerApp.Models;

namespace ComputerApp.Controllers
{
    public class ComputerOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComputerOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComputerOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ComputerOrder.Include(c => c.Computer).Include(c => c.Order);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ComputerOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerOrder = await _context.ComputerOrder
                .Include(c => c.Computer)
                .Include(c => c.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerOrder == null)
            {
                return NotFound();
            }

            return View(computerOrder);
        }

        // GET: ComputerOrders/Create
        public IActionResult Create()
        {
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Name");
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id");
            return View();
        }

        // POST: ComputerOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,ComputerId")] ComputerOrder computerOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computerOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Name", computerOrder.ComputerId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", computerOrder.OrderId);
            return View(computerOrder);
        }

        // GET: ComputerOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerOrder = await _context.ComputerOrder.FindAsync(id);
            if (computerOrder == null)
            {
                return NotFound();
            }
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Name", computerOrder.ComputerId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", computerOrder.OrderId);
            return View(computerOrder);
        }

        // POST: ComputerOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ComputerId")] ComputerOrder computerOrder)
        {
            if (id != computerOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computerOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerOrderExists(computerOrder.Id))
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
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Name", computerOrder.ComputerId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", computerOrder.OrderId);
            return View(computerOrder);
        }

        // GET: ComputerOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerOrder = await _context.ComputerOrder
                .Include(c => c.Computer)
                .Include(c => c.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerOrder == null)
            {
                return NotFound();
            }

            return View(computerOrder);
        }

        // POST: ComputerOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var computerOrder = await _context.ComputerOrder.FindAsync(id);
            _context.ComputerOrder.Remove(computerOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputerOrderExists(int id)
        {
            return _context.ComputerOrder.Any(e => e.Id == id);
        }
    }
}
