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

        public async Task<IActionResult> OnPostAsync(int? codigo, int? codigoMenu)
        {
            if (codigo == null || codigoMenu == null)
                return NotFound();

            var inv = await _context.Inventarios
                .FirstOrDefaultAsync(i =>
                    i.Codigo == codigo &&
                    i.CodigoMenu == codigoMenu);

            if (inv != null)
            {
                _context.Inventarios.Remove(inv);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ingrediente eliminado correctamente del inventario.";
            }
            return RedirectToPage("Index");
        }
    }
}
