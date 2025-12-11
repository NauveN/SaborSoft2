using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Mesas
{
    public class IndexModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public IndexModel(SaborCriolloContext context)
        {
            _context = context;
        }

        public class MesaEstadoViewModel
        {
            public int CodigoMesa { get; set; }
            public string Estado { get; set; } = "Libre";
            public string Horarios { get; set; } = "";
            public string Clientes { get; set; } = "";
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaFiltro { get; set; }

        public IList<MesaEstadoViewModel> Mesas { get; set; } = new List<MesaEstadoViewModel>();

        public async Task OnGetAsync()
        {
            var fecha = (FechaFiltro ?? DateTime.Today).Date;
            FechaFiltro = fecha;

            var mesasDelDia = await _context.Mesas
                .Include(m => m.Disponibilidad)
                .Include(m => m.CodigoReservaNavigation)
                    .ThenInclude(r => r.CedulaNavigation)
                .Include(m => m.CodigoReservaNavigation.FacturaReserva)
                .Where(m => m.CodigoReservaNavigation.Fecha == DateOnly.FromDateTime(fecha))
                .ToListAsync();

            for (int codigo = 1; codigo <= 10; codigo++)
            {
                var registros = mesasDelDia
                    .Where(m => m.Codigo == codigo)
                    .ToList();

                if (!registros.Any())
                {
                    Mesas.Add(new MesaEstadoViewModel
                    {
                        CodigoMesa = codigo,
                        Estado = "Libre"
                    });
                    continue;
                }

                var primero = registros.First();

                string estado = primero.CodigoReservaNavigation.FacturaReserva != null
                    ? "Ocupada"
                    : "Reservada";

                Mesas.Add(new MesaEstadoViewModel
                {
                    CodigoMesa = codigo,
                    Estado = estado,
                    Horarios = string.Join(", ", registros.Select(r => r.Disponibilidad.Disponibilidad1)),
                    Clientes = string.Join(", ", registros.Select(r => r.CodigoReservaNavigation.CedulaNavigation.Nombre))
                });
            }
        }
    }
}
