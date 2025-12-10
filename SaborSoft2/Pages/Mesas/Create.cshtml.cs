using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Mesas
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
        ViewData["CodigoReserva"] = new SelectList(_context.Reservas, "Codigo", "Codigo");
        ViewData["DisponibilidadId"] = new SelectList(_context.Disponibilidads, "DisponibilidadId", "DisponibilidadId");
            return Page();
        }

        [BindProperty]
        public Mesa Mesa { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Mesas.Add(Mesa);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
