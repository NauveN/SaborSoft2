using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Reservas
{
    public class EditModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public EditModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva =  await _context.Reservas.FirstOrDefaultAsync(m => m.Codigo == id);
            if (reserva == null)
            {
                return NotFound();
            }
            Reserva = reserva;
           ViewData["Cedula"] = new SelectList(_context.Usuarios, "Cedula", "Cedula");
           ViewData["TipoReservaId"] = new SelectList(_context.TipoReservas, "TipoReservaId", "TipoReservaId");
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

            _context.Attach(Reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(Reserva.Codigo))
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

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Codigo == id);
        }
    }
}
