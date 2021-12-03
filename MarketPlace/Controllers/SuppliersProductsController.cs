using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;

namespace MarketPlace.Controllers
{
    public class SuppliersProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SuppliersProducts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuppliersProducts.Include(s => s.Product).Include(s => s.Supplier);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SuppliersProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SuppliersProducts
                .Include(s => s.Product)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplierProduct == null)
            {
                return NotFound();
            }

            return View(supplierProduct);
        }

        [Authorize]
        // GET: SuppliersProducts/Create
        public IActionResult Create()
        {
            ViewData["productId"] = new SelectList(_context.Products, "Id", "Description");
            ViewData["supplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
            return View();
        }

        // POST: SuppliersProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,supplierId,productId")] SupplierProduct supplierProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplierProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productId"] = new SelectList(_context.Products, "Id", "Description", supplierProduct.productId);
            ViewData["supplierId"] = new SelectList(_context.Suppliers, "Id", "Name", supplierProduct.supplierId);
            return View(supplierProduct);
        }

        [Authorize]
        // GET: SuppliersProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SuppliersProducts.FindAsync(id);
            if (supplierProduct == null)
            {
                return NotFound();
            }
            ViewData["productId"] = new SelectList(_context.Products, "Id", "Description", supplierProduct.productId);
            ViewData["supplierId"] = new SelectList(_context.Suppliers, "Id", "Name", supplierProduct.supplierId);
            return View(supplierProduct);
        }

        // POST: SuppliersProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,supplierId,productId")] SupplierProduct supplierProduct)
        {
            if (id != supplierProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplierProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierProductExists(supplierProduct.Id))
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
            ViewData["productId"] = new SelectList(_context.Products, "Id", "Description", supplierProduct.productId);
            ViewData["supplierId"] = new SelectList(_context.Suppliers, "Id", "Name", supplierProduct.supplierId);
            return View(supplierProduct);
        }

        [Authorize]
        // GET: SuppliersProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SuppliersProducts
                .Include(s => s.Product)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplierProduct == null)
            {
                return NotFound();
            }

            return View(supplierProduct);
        }

        // POST: SuppliersProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplierProduct = await _context.SuppliersProducts.FindAsync(id);
            _context.SuppliersProducts.Remove(supplierProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierProductExists(int id)
        {
            return _context.SuppliersProducts.Any(e => e.Id == id);
        }
    }
}
