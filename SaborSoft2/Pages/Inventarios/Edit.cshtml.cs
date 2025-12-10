using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Inventarios
{
    public class EditModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public EditModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inventario Inventario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario =  await _context.Inventarios.FirstOrDefaultAsync(m => m.Codigo == id);
            if (inventario == null)
            {
                return NotFound();
            }
            Inventario = inventario;
           ViewData["Cedula"] = new SelectList(_context.Usuarios, "Cedula", "Cedula");
           ViewData["CodigoMenu"] = new SelectList(_context.Menus, "Codigo", "Codigo");
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

            _context.Attach(Inventario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventarioExists(Inventario.Codigo))
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

        private bool InventarioExists(int id)
        {
            return _context.Inventarios.Any(e => e.Codigo == id);
        }
    }
}
