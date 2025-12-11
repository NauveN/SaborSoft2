using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Reservas
{
    public class DeleteModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public DeleteModel(SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Mesas)
                .Include(r => r.FacturaReserva)
                .FirstOrDefaultAsync(r => r.Codigo == id);

            if (reserva != null)
            {
                if (reserva.Mesas != null && reserva.Mesas.Count > 0)
                    _context.Mesas.RemoveRange(reserva.Mesas);

                if (reserva.FacturaReserva != null)
                    _context.FacturaReservas.Remove(reserva.FacturaReserva);

                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
