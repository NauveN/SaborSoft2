using System;
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
        private readonly SaborCriolloContext _context;

        public EditModel(SaborCriolloContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reserva Reserva { get; set; } = default!;

        [BindProperty]
        public int? MesaCodigo { get; set; }

        [BindProperty]
        public int? DisponibilidadId { get; set; }

        public SelectList TiposReservaSelectList { get; set; } = default!;
        public SelectList MesasSelectList { get; set; } = default!;
        public SelectList DisponibilidadesSelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Reserva = await _context.Reservas
                .Include(r => r.Mesas)
                .FirstOrDefaultAsync(r => r.Codigo == id);

            if (Reserva == null)
                return NotFound();

            var mesa = Reserva.Mesas.FirstOrDefault();
            MesaCodigo = mesa?.Codigo;
            DisponibilidadId = mesa?.DisponibilidadId;

            await CargarCombosAsync();
            return Page();
        }

        private async Task CargarCombosAsync()
        {
            TiposReservaSelectList = new SelectList(
                await _context.TipoReservas
                    .OrderBy(t => t.TipoReserva1)
                    .ToListAsync(),
                "TipoReservaId",
                "TipoReserva1",
                Reserva.TipoReservaId);

            var todasLasMesas = Enumerable.Range(1, 10).ToList();
            MesasSelectList = new SelectList(todasLasMesas, MesaCodigo);

            DisponibilidadesSelectList = new SelectList(
                await _context.Disponibilidads
                    .OrderBy(d => d.Disponibilidad1)
                    .ToListAsync(),
                "DisponibilidadId",
                "DisponibilidadId",
                DisponibilidadId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarCombosAsync();

            // Quitar navegaciones/código de la validación automática
            ModelState.Remove("Reserva.CedulaNavigation");
            ModelState.Remove("Reserva.TipoReserva");
            ModelState.Remove("Reserva.Mesas");
            ModelState.Remove("Reserva.FacturaReserva");
            ModelState.Remove("Reserva.CodigoUnico");

            if (Reserva.Fecha.ToDateTime(TimeOnly.MinValue) < DateTime.Today)
                ModelState.AddModelError("Reserva.Fecha", "La fecha debe ser hoy o una fecha futura.");

            if (Reserva.Cantidad <= 0)
                ModelState.AddModelError("Reserva.Cantidad", "La cantidad de personas debe ser mayor que cero.");

            if (Reserva.TipoReservaId == 0)
                ModelState.AddModelError("Reserva.TipoReservaId", "Seleccione un tipo de reserva.");

            if (!MesaCodigo.HasValue)
                ModelState.AddModelError(nameof(MesaCodigo), "Seleccione una mesa.");

            if (!DisponibilidadId.HasValue)
                ModelState.AddModelError(nameof(DisponibilidadId), "Seleccione un horario.");

            // una reserva por cédula y fecha, excluyendo la actual
            bool yaTiene = await _context.Reservas
                .AnyAsync(r => r.Cedula == Reserva.Cedula &&
                               r.Fecha == Reserva.Fecha &&
                               r.Codigo != Reserva.Codigo);

            if (yaTiene)
                ModelState.AddModelError(string.Empty,
                    "Esta cédula ya tiene otra reserva en la fecha seleccionada.");

            // mesa libre, excluyendo la mesa actual de esta reserva
            if (MesaCodigo.HasValue && DisponibilidadId.HasValue)
            {
                bool mesaOcupada = await _context.Mesas
                    .Include(m => m.CodigoReservaNavigation)
                    .AnyAsync(m =>
                        m.Codigo == MesaCodigo.Value &&
                        m.DisponibilidadId == DisponibilidadId.Value &&
                        m.CodigoReservaNavigation.Fecha == Reserva.Fecha &&
                        m.CodigoReserva != Reserva.Codigo);

                if (mesaOcupada)
                    ModelState.AddModelError(string.Empty,
                        "La mesa seleccionada ya está reservada en ese horario para esa fecha.");
            }

            if (!ModelState.IsValid)
                return Page();

            var reservaDb = await _context.Reservas
                .Include(r => r.Mesas)
                .FirstOrDefaultAsync(r => r.Codigo == Reserva.Codigo);

            if (reservaDb == null)
                return NotFound();

            // actualizar datos principales
            reservaDb.Fecha = Reserva.Fecha;
            reservaDb.Cantidad = Reserva.Cantidad;
            reservaDb.TipoReservaId = Reserva.TipoReservaId;
            reservaDb.Cedula = Reserva.Cedula;

            // actualizar mesa
            var mesa = reservaDb.Mesas.FirstOrDefault();
            if (mesa != null)
            {
                mesa.Codigo = MesaCodigo!.Value;
                mesa.DisponibilidadId = DisponibilidadId!.Value;
                mesa.Cantidad = Reserva.Cantidad;
            }
            else
            {
                reservaDb.Mesas.Add(new Mesa
                {
                    Codigo = MesaCodigo!.Value,
                    CodigoReserva = reservaDb.Codigo,
                    Cantidad = reservaDb.Cantidad,
                    DisponibilidadId = DisponibilidadId!.Value
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
