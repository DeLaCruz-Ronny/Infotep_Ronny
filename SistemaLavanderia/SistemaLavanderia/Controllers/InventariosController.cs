using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Controllers
{
    public class InventariosController : Controller
    {
        private readonly LavanderiaContext _context;

        public InventariosController(LavanderiaContext context)
        {
            _context = context;
        }

        // GET: Inventarios
        public async Task<IActionResult> Index()
        {
            var lavanderiaContext = _context.Inventarios.Include(i => i.CategoriaNavigation).Include(i => i.ServicioNavigation);
            return View(await lavanderiaContext.ToListAsync());
        }

        // GET: Inventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios
                .Include(i => i.CategoriaNavigation)
                .Include(i => i.ServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdProductos == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // GET: Inventarios/Create
        public IActionResult Create()
        {
            ViewData["Categoria"] = new SelectList(_context.CatProductos, "IdCatProducto", "DescatProducto");
            ViewData["Servicio"] = new SelectList(_context.Servicios, "IdServicio", "DesServicio");
            return View();
        }

        // POST: Inventarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProductos,DesProductos,Categoria,CantidadProducto,Servicio")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoria"] = new SelectList(_context.CatProductos, "IdCatProducto", "DescatProducto", inventario.Categoria);
            ViewData["Servicio"] = new SelectList(_context.Servicios, "IdServicio", "DesServicio", inventario.Servicio);
            return View(inventario);
        }

        // GET: Inventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            ViewData["Categoria"] = new SelectList(_context.CatProductos, "IdCatProducto", "DescatProducto", inventario.Categoria);
            ViewData["Servicio"] = new SelectList(_context.Servicios, "IdServicio", "DesServicio", inventario.Servicio);
            return View(inventario);
        }

        // POST: Inventarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProductos,DesProductos,Categoria,CantidadProducto,Servicio")] Inventario inventario)
        {
            if (id != inventario.IdProductos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(inventario.IdProductos))
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
            ViewData["Categoria"] = new SelectList(_context.CatProductos, "IdCatProducto", "DescatProducto", inventario.Categoria);
            ViewData["Servicio"] = new SelectList(_context.Servicios, "IdServicio", "DesServicio", inventario.Servicio);
            return View(inventario);
        }

        // GET: Inventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios
                .Include(i => i.CategoriaNavigation)
                .Include(i => i.ServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdProductos == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario != null)
            {
                _context.Inventarios.Remove(inventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventarios.Any(e => e.IdProductos == id);
        }
    }
}
