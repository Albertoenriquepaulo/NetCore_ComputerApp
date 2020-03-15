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
    public class ComputerComponentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComputerComponentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComputerComponents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ComputerComponent.Include(c => c.Component).Include(c => c.Computer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ComputerComponents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerComponent = await _context.ComputerComponent
                .Include(c => c.Component)
                .Include(c => c.Computer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerComponent == null)
            {
                return NotFound();
            }

            return View(computerComponent);
        }

        // GET: ComputerComponents/Create
        public IActionResult Create()
        {
            ViewData["ComponentId"] = new SelectList(_context.Component, "Id", "Name");
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Name");
            return View();
        }

        // POST: ComputerComponents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ComputerId,ComponentId")] ComputerComponent computerComponent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computerComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComponentId"] = new SelectList(_context.Component, "Id", "Name", computerComponent.ComponentId);
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Name", computerComponent.ComputerId);
            return View(computerComponent);
        }

        // GET: ComputerComponents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerComponent = await _context.ComputerComponent.FindAsync(id);
            if (computerComponent == null)
            {
                return NotFound();
            }
            ViewData["ComponentId"] = new SelectList(_context.Component, "Id", "Name", computerComponent.ComponentId);
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Name", computerComponent.ComputerId);
            return View(computerComponent);
        }

        // POST: ComputerComponents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ComputerId,ComponentId")] ComputerComponent computerComponent)
        {
            if (id != computerComponent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computerComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerComponentExists(computerComponent.Id))
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
            ViewData["ComponentId"] = new SelectList(_context.Component, "Id", "Name", computerComponent.ComponentId);
            ViewData["ComputerId"] = new SelectList(_context.Computer, "Id", "Name", computerComponent.ComputerId);
            return View(computerComponent);
        }

        // GET: ComputerComponents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var computerComponent = await _context.ComputerComponent
                .Include(c => c.Component)
                .Include(c => c.Computer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerComponent == null)
            {
                return NotFound();
            }

            return View(computerComponent);
        }

        // POST: ComputerComponents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var computerComponent = await _context.ComputerComponent.FindAsync(id);
            _context.ComputerComponent.Remove(computerComponent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputerComponentExists(int id)
        {
            return _context.ComputerComponent.Any(e => e.Id == id);
        }
    }
}
