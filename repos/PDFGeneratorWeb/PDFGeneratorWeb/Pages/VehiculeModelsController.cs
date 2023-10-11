using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PDFGeneratorWeb.Data;

namespace PDFGeneratorWeb.Pages
{
    public class VehiculeModelsController : Controller
    {
        private readonly FactoryContext _context;

        public VehiculeModelsController(FactoryContext context)
        {
            _context = context;
        }

        // GET: VehiculeModels
        public async Task<IActionResult> Index()
        {
              return _context.VehiculeModel != null ? 
                          View(await _context.VehiculeModel.ToListAsync()) :
                          Problem("Entity set 'FactoryContext.VehiculeModel'  is null.");
        }

        // GET: VehiculeModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VehiculeModel == null)
            {
                return NotFound();
            }

            var vehiculeModel = await _context.VehiculeModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculeModel == null)
            {
                return NotFound();
            }

            return View(vehiculeModel);
        }

        // GET: VehiculeModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehiculeModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] VehiculeModel vehiculeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehiculeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehiculeModel);
        }

        // GET: VehiculeModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VehiculeModel == null)
            {
                return NotFound();
            }

            var vehiculeModel = await _context.VehiculeModel.FindAsync(id);
            if (vehiculeModel == null)
            {
                return NotFound();
            }
            return View(vehiculeModel);
        }

        // POST: VehiculeModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] VehiculeModel vehiculeModel)
        {
            if (id != vehiculeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiculeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculeModelExists(vehiculeModel.Id))
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
            return View(vehiculeModel);
        }

        // GET: VehiculeModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VehiculeModel == null)
            {
                return NotFound();
            }

            var vehiculeModel = await _context.VehiculeModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculeModel == null)
            {
                return NotFound();
            }

            return View(vehiculeModel);
        }

        // POST: VehiculeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VehiculeModel == null)
            {
                return Problem("Entity set 'FactoryContext.VehiculeModel'  is null.");
            }
            var vehiculeModel = await _context.VehiculeModel.FindAsync(id);
            if (vehiculeModel != null)
            {
                _context.VehiculeModel.Remove(vehiculeModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculeModelExists(int id)
        {
          return (_context.VehiculeModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
