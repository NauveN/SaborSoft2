using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Inventarios
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
        ViewData["Cedula"] = new SelectList(_context.Usuarios, "Cedula", "Cedula");
        ViewData["CodigoMenu"] = new SelectList(_context.Menus, "Codigo", "Codigo");
            return Page();
        }

        [BindProperty]
        public Inventario Inventario { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Inventarios.Add(Inventario);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
