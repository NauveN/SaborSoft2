using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Reservas
{
    public class DetailsModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public DetailsModel(SaborCriolloContext context)
        {
            _context = context;
        }

        public Reserva Reserva { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Reserva = await _context.Reservas
                .Include(r => r.CedulaNavigation)
                .Include(r => r.TipoReserva)
                .Include(r => r.Mesas)
                    .ThenInclude(m => m.Disponibilidad)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (Reserva == null)
                return NotFound();

            return Page();
        }
    }
}
