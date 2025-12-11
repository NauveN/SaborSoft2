using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Usuarios
{
    public class CreateModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public CreateModel(SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = new Usuario();

        public SelectList TiposUsuarioSelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await CargarTiposAsync();
            return Page();
        }

        private async Task CargarTiposAsync()
        {
            var tipos = await _context.TipoUsuarios
                .OrderBy(t => t.TipoUsuario1)
                .ToListAsync();

            // value = TipoUsuarioId, text = TipoUsuario1 (Administrador, Empleado, Cliente, etc.)
            TiposUsuarioSelectList = new SelectList(tipos, "TipoUsuarioId", "TipoUsuario1");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarTiposAsync();

            // 1) Quitar validación automática de navegaciones/colecciones
            ModelState.Remove("Usuario.TipoUsuario");
            ModelState.Remove("Usuario.Inventarios");
            ModelState.Remove("Usuario.Reservas");

            // 2) Validaciones manuales

            if (string.IsNullOrWhiteSpace(Usuario.Cedula))
                ModelState.AddModelError("Usuario.Cedula", "La cédula es obligatoria.");

            if (string.IsNullOrWhiteSpace(Usuario.Nombre))
                ModelState.AddModelError("Usuario.Nombre", "El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(Usuario.CorreoElectronico))
                ModelState.AddModelError("Usuario.CorreoElectronico", "El correo es obligatorio.");

            if (Usuario.TipoUsuarioId == 0)
                ModelState.AddModelError("Usuario.TipoUsuarioId", "Seleccione un tipo de usuario.");

            // Cédula única
            bool existe = await _context.Usuarios.AnyAsync(u => u.Cedula == Usuario.Cedula);
            if (existe)
                ModelState.AddModelError("Usuario.Cedula", "Ya existe un usuario con esta cédula.");

            if (!ModelState.IsValid)
                return Page();

            Usuario.FechaRegistro = DateTime.Now;

            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Usuario registrado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
