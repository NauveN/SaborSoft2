using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Reservas
{
    public class IndexModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public IndexModel(SaborCriolloContext context)
        {
            _context = context;
        }

        public IList<Reserva> Reservas { get; set; } = new List<Reserva>();

        [BindProperty(SupportsGet = true)]
        public string? CedulaFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaFiltro { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Reservas
                .Include(r => r.CedulaNavigation)
                .Include(r => r.TipoReserva)
                .Include(r => r.Mesas)
                    .ThenInclude(m => m.Disponibilidad)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(CedulaFiltro))
            {
                query = query.Where(r => r.Cedula.Contains(CedulaFiltro));
            }

            if (FechaFiltro.HasValue)
            {
                var fecha = DateOnly.FromDateTime(FechaFiltro.Value.Date);
                query = query.Where(r => r.Fecha == fecha);
            }

            Reservas = await query
                .OrderBy(r => r.Fecha)
                .ThenBy(r => r.Cedula)
                .ToListAsync();
        }
    }
}
