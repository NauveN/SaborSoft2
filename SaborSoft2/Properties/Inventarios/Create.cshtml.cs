using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Inventarios
{
    public class CreateModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public CreateModel(SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inventario Inventario { get; set; } = new Inventario();

        // Responsable (Administrador / Empleado)
        [BindProperty]
        public string CedulaResponsable { get; set; } = string.Empty;

        public SelectList UsuariosSelectList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (Inventario.FechaAdquisicion == default)
                Inventario.FechaAdquisicion = DateOnly.FromDateTime(DateTime.Today);

            await CargarUsuariosAsync();
        }

        private async Task CargarUsuariosAsync()
        {
            // Usuarios que NO son clientes (Administrador / Mesero / Empleado...)
            var usuarios = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .Where(u => u.TipoUsuario.TipoUsuario1 != "Cliente")
                .OrderBy(u => u.Nombre)
                .ToListAsync();

            UsuariosSelectList = new SelectList(usuarios, "Cedula", "Nombre");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarUsuariosAsync();

            // Quitamos validaciones automáticas de cosas que no vienen del form
            ModelState.Remove("Inventario.CedulaNavigation");
            ModelState.Remove("Inventario.CodigoMenuNavigation");
            ModelState.Remove("Inventario.Cedula");   // la asignamos nosotros

            // VALIDACIONES MANUALES

            if (string.IsNullOrWhiteSpace(CedulaResponsable))
                ModelState.AddModelError(nameof(CedulaResponsable), "Debe seleccionar un responsable.");

            if (string.IsNullOrWhiteSpace(Inventario.Descripcion))
                ModelState.AddModelError("Inventario.Descripcion", "El nombre del ingrediente es obligatorio.");

            if (Inventario.Stock < 0)
                ModelState.AddModelError("Inventario.Stock", "La cantidad inicial no puede ser negativa.");

            if (string.IsNullOrWhiteSpace(Inventario.UnidadMedida))
                ModelState.AddModelError("Inventario.UnidadMedida", "La unidad de medida es obligatoria.");

            if (Inventario.FechaAdquisicion > DateOnly.FromDateTime(DateTime.Today))
                ModelState.AddModelError("Inventario.FechaAdquisicion", "La fecha de adquisición no puede ser futura.");

            if (Inventario.StockMinimo < 0)
                ModelState.AddModelError("Inventario.StockMinimo", "El stock mínimo no puede ser negativo.");

            // Validar que no exista el mismo ingrediente (mismo nombre)
            bool existe = await _context.Inventarios
                .AnyAsync(i => i.Descripcion == Inventario.Descripcion);

            if (existe)
                ModelState.AddModelError("Inventario.Descripcion",
                    "Ya existe un ingrediente con este nombre en el inventario.");

            if (!ModelState.IsValid)
                return Page();

            // Asignamos el responsable
            Inventario.Cedula = CedulaResponsable;
            Inventario.FechaActualizacion = DateTime.Now;

            _context.Inventarios.Add(Inventario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ingrediente registrado correctamente en el inventario.";

            return RedirectToPage("Index");
        }
    }
}
