using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MarketPlace.ViewsModels;
using Microsoft.AspNetCore.Authorization;

namespace MarketPlace.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchByName, int? categoryId)
        {
            var appDBcontext = _context.Products.Include(a => a.Category).Select(a => a);
            
            if (!string.IsNullOrEmpty(searchByName))
            {
                appDBcontext = appDBcontext.Where(a => a.Name.Contains(searchByName));
            }

            if (categoryId.HasValue)
            {
                appDBcontext = appDBcontext.Where(a => a.categoryId == categoryId.Value);
            }

            ProductsViewModel model = new()
            {
                Products = appDBcontext.ToList(),
                Categories = new SelectList(_context.Categories, "Id", "Name", categoryId),
                Name = searchByName

            };

            return View(model);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["brandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["categoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Image,Favorite,categoryId,brandId")] Product product)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    var photoFile = files[0];
                    if (photoFile.Length > 0)
                    {
                        var destinationPath = Path.Combine(_env.WebRootPath, "images\\products");
                        var destinationFile = Guid.NewGuid().ToString();
                        destinationFile = destinationFile.Replace("-", "");
                        destinationFile += Path.GetExtension(photoFile.FileName);
                        var destinationRoute = Path.Combine(destinationPath, destinationFile);

                        using (var filestream = new FileStream(destinationRoute, FileMode.Create))
                        {
                            photoFile.CopyTo(filestream);
                            product.Image = destinationFile;
                        };
                    }
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["brandId"] = new SelectList(_context.Brands, "Id", "Name", product.brandId);
            ViewData["categoryId"] = new SelectList(_context.Categories, "Id", "Name", product.categoryId);
            return View(product);
        }

        [Authorize]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["brandId"] = new SelectList(_context.Brands, "Id", "Name", product.brandId);
            ViewData["categoryId"] = new SelectList(_context.Categories, "Id", "Name", product.categoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,Image,Favorite,categoryId,brandId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files != null && files.Count > 0)
                    {
                        var photoFile = files[0];
                        if (photoFile.Length > 0)
                        {
                            var destinationPath = Path.Combine(_env.WebRootPath, "images\\products");
                            var destinationFile = Guid.NewGuid().ToString();
                            destinationFile = destinationFile.Replace("-", "");
                            destinationFile += Path.GetExtension(photoFile.FileName);
                            var destinationRoute = Path.Combine(destinationPath, destinationFile);

                            if (!string.IsNullOrEmpty(product.Image))
                            {
                                string previousPhoto = Path.Combine(destinationPath, product.Image);
                                if (System.IO.File.Exists(previousPhoto))
                                    System.IO.File.Delete(previousPhoto);
                            }

                            using (var filestream = new FileStream(destinationRoute, FileMode.Create))
                            {
                                photoFile.CopyTo(filestream);
                                product.Image = destinationFile;
                            };
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["brandId"] = new SelectList(_context.Brands, "Id", "Name", product.brandId);
            ViewData["categoryId"] = new SelectList(_context.Categories, "Id", "Name", product.categoryId);
            return View(product);
        }

        [Authorize]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
