using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JDMR20241103.Data;
using JDMR20241103.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JDMR20241103.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
              return _context.Proveedores != null ? 
                          View(await _context.Proveedores.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Proveedores'  is null.");
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
                 .Include(s => s.DetalleProveedores)
                 .FirstOrDefaultAsync(m => m.Id == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            var proveedor = new Proveedor();
            proveedor.DetalleProveedores = new List<DetalleProveedor>();
            proveedor.DetalleProveedores.Add(new DetalleProveedor
            {
                Calle = "",
                Ciudad = "",
                Estado = "",
                CodigoPostal = "",
                Pais = ""
            });
            ViewBag.Accion = "Create";
            return View(proveedor);
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Telefono,Descripcion,DetalleProveedores")] Proveedor proveedor)
        {
            _context.Add(proveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #region DETALLES
        [HttpPost]
        public ActionResult AgregarDetalles([Bind("Id,Nombre,Nombre,Telefono,Descripcion,DetalleProveedores")] Proveedor proveedor, string accion)
        {
            proveedor.DetalleProveedores.Add(new DetalleProveedor
            {
                Calle = "",
                Ciudad = "",
                Estado = "",
                CodigoPostal = "",
                Pais = ""
            });
            ViewBag.Accion = accion;
            return View(accion, proveedor);
        }

        public ActionResult EliminarDetalles([Bind("Id,Nombre,Nombre,Telefono,Descripcion,DetalleProveedores")] Proveedor proveedor, string accion, int index)
        {
            var det = proveedor.DetalleProveedores[index];
            if (accion == "Edit" && det.Id > 0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                proveedor.DetalleProveedores.RemoveAt(index);
            }

            ViewBag.Accion = accion;
            return View(accion, proveedor);
        }
        #endregion

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
               .Include(s => s.DetalleProveedores)
               .FirstAsync(s => s.Id == id);

            if (proveedor == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Telefono,Descripcion,DetalleProveedores")] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }
            try
            {
                // Obtener los datos de la base de datos que van a ser modificados
                var proveedorUpdate = await _context.Proveedores
                        .Include(s => s.DetalleProveedores)
                        .FirstAsync(s => s.Id == proveedor.Id);
                proveedorUpdate.Nombre = proveedor.Nombre;
                proveedorUpdate.Telefono = proveedor.Telefono;
                proveedorUpdate.Descripcion = proveedor.Descripcion;
                // Obtener todos los detalles que seran nuevos y agregarlos a la base de datos
                var detNew = proveedor.DetalleProveedores.Where(s => s.Id == 0);
                foreach (var d in detNew)
                {
                    proveedorUpdate.DetalleProveedores.Add(d);
                }
                // Obtener todos los detalles que seran modificados y actualizar a la base de datos
                var detUpdate = proveedor.DetalleProveedores.Where(s => s.Id > 0);
                foreach (var d in detUpdate)
                {
                    var det = proveedorUpdate.DetalleProveedores.FirstOrDefault(s => s.Id == d.Id);
                    det.Calle = d.Calle;
                    det.Ciudad = d.Ciudad;
                    det.Estado = d.Estado;
                    det.CodigoPostal = d.CodigoPostal;
                    det.Pais = d.Pais;
                }
                // Obtener todos los detalles que seran eliminados y actualizar a la base de datos
                var delDetIds = proveedor.DetalleProveedores.Where(s => s.Id < 0).Select(s => -s.Id).ToList();
                if (delDetIds != null && delDetIds.Count > 0)
                {
                    foreach (var detalleId in delDetIds) // Cambiado de 'id' a 'detalleId'
                    {
                        var det = await _context.DetalleProveedores.FindAsync(detalleId); // Cambiado de 'id' a 'detalleId'
                        if (det != null)
                        {
                            _context.DetalleProveedores.Remove(det);
                        }
                    }
                }
                // Aplicar esos cambios a la base de datos
                _context.Update(proveedorUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedorExists(proveedor.Id))
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

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }
            var proveedor = await _context.Proveedores
                .Include(s => s.DetalleProveedores)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (proveedor == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Delete";
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proveedores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Proveedores'  is null.");
            }
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
          return (_context.Proveedores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
