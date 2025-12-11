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
    public class CreateModel : PageModel
    {
        private readonly SaborCriolloContext _context;
        private static readonly char[] CodigoChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private readonly Random _random = new Random();

        public CreateModel(SaborCriolloContext context)
        {
            _context = context;
        }

        // ENTIDAD RESERVA
        [BindProperty]
        public Reserva Reserva { get; set; } = new Reserva();

        // Datos del cliente
        [BindProperty]
        public string NombreCliente { get; set; } = string.Empty;

        [BindProperty]
        public string CorreoCliente { get; set; } = string.Empty;

        // Mesa y horario
        [BindProperty]
        public int? MesaCodigo { get; set; }

        [BindProperty]
        public int? DisponibilidadId { get; set; }

        public SelectList TiposReservaSelectList { get; set; } = default!;
        public SelectList MesasSelectList { get; set; } = default!;
        public SelectList DisponibilidadesSelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (Reserva.Fecha == default)
                Reserva.Fecha = DateOnly.FromDateTime(DateTime.Today);

            await CargarCombosAsync();
            return Page();
        }

        // --------- Helpers ---------

        private async Task<string> GenerarCodigoReservaAsync()
        {
            string code;
            bool existe;

            do
            {
                var buffer = new char[6];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = CodigoChars[_random.Next(CodigoChars.Length)];
                }

                code = new string(buffer);

                existe = await _context.Reservas.AnyAsync(r => r.CodigoUnico == code);
            }
            while (existe);

            return code;
        }

        private async Task CargarCombosAsync()
        {
            // Tipos de reserva
            TiposReservaSelectList = new SelectList(
                await _context.TipoReservas
                    .OrderBy(t => t.TipoReserva1)
                    .ToListAsync(),
                "TipoReservaId",
                "TipoReserva1",
                Reserva.TipoReservaId);

            // Horarios
            var disponibilidades = await _context.Disponibilidads
                .OrderBy(d => d.Disponibilidad1)
                .ToListAsync();
            DisponibilidadesSelectList = new SelectList(
                disponibilidades,
                "DisponibilidadId",
                "Disponibilidad1",
                DisponibilidadId);

            // Mesas disponibles (1..10 menos las ya reservadas ese día/horario)
            var todasLasMesas = Enumerable.Range(1, 10).ToList();
            var disponibles = todasLasMesas;

            if (Reserva.Fecha != default && DisponibilidadId.HasValue)
            {
                var ocupadas = await _context.Mesas
                    .Include(m => m.CodigoReservaNavigation)
                    .Where(m => m.CodigoReservaNavigation.Fecha == Reserva.Fecha
                             && m.DisponibilidadId == DisponibilidadId.Value)
                    .Select(m => m.Codigo)
                    .Distinct()
                    .ToListAsync();

                disponibles = todasLasMesas.Except(ocupadas).ToList();

                if (!disponibles.Any())
                {
                    ModelState.AddModelError(string.Empty,
                        "No hay mesas disponibles para la fecha y horario seleccionados.");
                }
            }

            MesasSelectList = new SelectList(disponibles, MesaCodigo);
        }

        // --------- POST: Registrar reserva ---------

        public async Task<IActionResult> OnPostAsync()
        {
            await CargarCombosAsync();

            // Quitar validaciones automáticas que no usamos en el formulario
            ModelState.Remove("Reserva.CedulaNavigation");
            ModelState.Remove("Reserva.TipoReserva");
            ModelState.Remove("Reserva.Mesas");
            ModelState.Remove("Reserva.FacturaReserva");
            ModelState.Remove("Reserva.CodigoUnico");

            // VALIDACIONES MANUALES

            if (string.IsNullOrWhiteSpace(Reserva.Cedula))
                ModelState.AddModelError("Reserva.Cedula", "La cédula es obligatoria.");

            if (string.IsNullOrWhiteSpace(NombreCliente))
                ModelState.AddModelError(nameof(NombreCliente), "El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(CorreoCliente))
                ModelState.AddModelError(nameof(CorreoCliente), "El correo es obligatorio.");

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

            // Una reserva por cédula y día
            if (!string.IsNullOrWhiteSpace(Reserva.Cedula))
            {
                bool yaTiene = await _context.Reservas
                    .AnyAsync(r => r.Cedula == Reserva.Cedula && r.Fecha == Reserva.Fecha);

                if (yaTiene)
                    ModelState.AddModelError(string.Empty,
                        "Esta cédula ya tiene una reserva para la fecha seleccionada.");
            }

            // Seguridad extra: mesa no ocupada
            if (MesaCodigo.HasValue && DisponibilidadId.HasValue)
            {
                bool mesaOcupada = await _context.Mesas
                    .Include(m => m.CodigoReservaNavigation)
                    .AnyAsync(m =>
                        m.Codigo == MesaCodigo.Value &&
                        m.DisponibilidadId == DisponibilidadId.Value &&
                        m.CodigoReservaNavigation.Fecha == Reserva.Fecha);

                if (mesaOcupada)
                    ModelState.AddModelError(string.Empty,
                        "La mesa seleccionada ya está reservada en ese horario para esa fecha.");
            }

            if (!ModelState.IsValid)
                return Page();

            // Crear / actualizar usuario tipo CLIENTE
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Cedula == Reserva.Cedula);

            if (usuario == null)
            {
                int tipoClienteId = await _context.TipoUsuarios
                    .Where(t => t.TipoUsuario1 == "Cliente")
                    .Select(t => t.TipoUsuarioId)
                    .FirstOrDefaultAsync();

                if (tipoClienteId == 0)
                {
                    var tipoCliente = new TipoUsuario { TipoUsuario1 = "Cliente" };
                    _context.TipoUsuarios.Add(tipoCliente);
                    await _context.SaveChangesAsync();
                    tipoClienteId = tipoCliente.TipoUsuarioId;
                }

                usuario = new Usuario
                {
                    Cedula = Reserva.Cedula,
                    Nombre = NombreCliente,
                    CorreoElectronico = CorreoCliente,
                    FechaRegistro = DateTime.Now,
                    TipoUsuarioId = tipoClienteId
                };
                _context.Usuarios.Add(usuario);
            }
            else
            {
                usuario.Nombre = NombreCliente;
                usuario.CorreoElectronico = CorreoCliente;
                _context.Usuarios.Update(usuario);
            }

            await _context.SaveChangesAsync();

            // Generar código alfanumérico único
            Reserva.CodigoUnico = await GenerarCodigoReservaAsync();

            // Guardar reserva
            _context.Reservas.Add(Reserva);
            await _context.SaveChangesAsync();

            // Guardar mesa asignada
            var mesaReserva = new Mesa
            {
                Codigo = MesaCodigo!.Value,
                CodigoReserva = Reserva.Codigo,
                Cantidad = Reserva.Cantidad,
                DisponibilidadId = DisponibilidadId!.Value
            };

            _context.Mesas.Add(mesaReserva);
            await _context.SaveChangesAsync();

            return RedirectToPage("Details", new { id = Reserva.Codigo });
        }
    }
}
