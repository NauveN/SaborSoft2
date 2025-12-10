using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Mesas
{
    public class EditModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public EditModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Mesa Mesa { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa =  await _context.Mesas.FirstOrDefaultAsync(m => m.Codigo == id);
            if (mesa == null)
            {
                return NotFound();
            }
            Mesa = mesa;
           ViewData["CodigoReserva"] = new SelectList(_context.Reservas, "Codigo", "Codigo");
           ViewData["DisponibilidadId"] = new SelectList(_context.Disponibilidads, "DisponibilidadId", "DisponibilidadId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Mesa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MesaExists(Mesa.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.Codigo == id);
        }
    }
}
