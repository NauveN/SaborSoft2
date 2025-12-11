using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Inventarios
{
    public class DeleteModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public DeleteModel(SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inventario Inventario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var inv = await _context.Inventarios.FirstOrDefaultAsync(i => i.Codigo == id);

            if (inv == null)
                return NotFound();

            Inventario = inv;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var inv = await _context.Inventarios.FindAsync(id);

            if (inv != null)
            {
                // Aquí se podría registrar en una tabla de auditoría
                _context.Inventarios.Remove(inv);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ingrediente eliminado correctamente del inventario.";
            }

            return RedirectToPage("Index");
        }
    }
}
