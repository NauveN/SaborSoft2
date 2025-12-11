using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SaborSoft2.Models;

namespace SaborSoft2
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SaborCriolloContext>();

            // Por si acaso
            context.Database.EnsureCreated();

            // TIPOS DE USUARIO
            if (!context.TipoUsuarios.Any())
            {
                context.TipoUsuarios.AddRange(
                    new TipoUsuario { TipoUsuario1 = "Administrador" },
                    new TipoUsuario { TipoUsuario1 = "Mesero" },
                    new TipoUsuario { TipoUsuario1 = "Cliente" }
                );
            }

            // TIPOS DE RESERVA
            if (!context.TipoReservas.Any())
            {
                context.TipoReservas.AddRange(
                    new TipoReserva { TipoReserva1 = "Reserva en salón" },
                    new TipoReserva { TipoReserva1 = "Evento" },
                    new TipoReserva { TipoReserva1 = "Cumpleaños" }
                );
            }

            // DISPONIBILIDADES (HORARIOS)
            if (!context.Disponibilidads.Any())
            {
                context.Disponibilidads.AddRange(
                    new Disponibilidad { Disponibilidad1 = "12:00 - 13:00" },
                    new Disponibilidad { Disponibilidad1 = "13:00 - 14:00" },
                    new Disponibilidad { Disponibilidad1 = "19:00 - 20:00" },
                    new Disponibilidad { Disponibilidad1 = "20:00 - 21:00" }
                );
            }

            context.SaveChanges();
        }
    }
}
