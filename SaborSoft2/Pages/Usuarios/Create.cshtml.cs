using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Usuarios
{
    public class CreateModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public CreateModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["TipoUsuarioId"] = new SelectList(_context.TipoUsuarios, "TipoUsuarioId", "TipoUsuarioId");
            return Page();
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
