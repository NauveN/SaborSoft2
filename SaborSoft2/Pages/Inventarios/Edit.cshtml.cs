using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Inventarios
{
    public class EditModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public EditModel(SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inventario Inventario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? codigo, int? codigoMenu)
        {
            if (codigo == null || codigoMenu == null)
                return NotFound();

            var inv = await _context.Inventarios
                .AsNoTracking()
                .FirstOrDefaultAsync(i =>
                    i.Codigo == codigo &&
                    i.CodigoMenu == codigoMenu);

            if (inv == null)
                return NotFound();

            Inventario = inv;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Quitar cosas que no vienen del form
            ModelState.Remove("Inventario.CedulaNavigation");
            ModelState.Remove("Inventario.CodigoMenuNavigation");
            ModelState.Remove("Inventario.Cedula");

            if (Inventario.Stock < 0)
                ModelState.AddModelError("Inventario.Stock", "La cantidad no puede ser negativa.");

            if (Inventario.StockMinimo < 0)
                ModelState.AddModelError("Inventario.StockMinimo", "El stock mínimo no puede ser negativo.");

            if (!ModelState.IsValid)
                return Page();

            var invDb = await _context.Inventarios
                .FirstOrDefaultAsync(i =>
                    i.Codigo == Inventario.Codigo &&
                    i.CodigoMenu == Inventario.CodigoMenu);

            if (invDb == null)
                return NotFound();

            // Actualizar solo campos editables
            invDb.Descripcion = Inventario.Descripcion;
            invDb.Stock = Inventario.Stock;
            invDb.UnidadMedida = Inventario.UnidadMedida;
            invDb.FechaAdquisicion = Inventario.FechaAdquisicion;
            invDb.StockMinimo = Inventario.StockMinimo;
            invDb.FechaActualizacion = DateTime.Now;
            // OJO: no tocamos invDb.Cedula ni invDb.CodigoMenu

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Inventario actualizado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
