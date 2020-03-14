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
    public class CTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CType.ToListAsync());
        }

        // GET: CTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cType = await _context.CType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cType == null)
            {
                return NotFound();
            }

            return View(cType);
        }

        // GET: CTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CType cType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cType);
        }

        // GET: CTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cType = await _context.CType.FindAsync(id);
            if (cType == null)
            {
                return NotFound();
            }
            return View(cType);
        }

        // POST: CTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CType cType)
        {
            if (id != cType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CTypeExists(cType.Id))
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
            return View(cType);
        }

        // GET: CTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cType = await _context.CType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cType == null)
            {
                return NotFound();
            }

            return View(cType);
        }

        // POST: CTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cType = await _context.CType.FindAsync(id);
            _context.CType.Remove(cType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CTypeExists(int id)
        {
            return _context.CType.Any(e => e.Id == id);
        }
    }
}
