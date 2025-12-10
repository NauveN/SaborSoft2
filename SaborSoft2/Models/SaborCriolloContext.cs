using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

public partial class SaborCriolloContext : DbContext
{
    public SaborCriolloContext()
    {
    }

    public SaborCriolloContext(DbContextOptions<SaborCriolloContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Disponibilidad> Disponibilidads { get; set; }

    public virtual DbSet<FacturaReserva> FacturaReservas { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<TipoReserva> TipoReservas { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SaborCriolloDb;Trusted_Connection=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Disponibilidad>(entity =>
        {
            entity.HasKey(e => e.DisponibilidadId).HasName("PK__Disponib__6CF1672855ED7C36");
        });

        modelBuilder.Entity<FacturaReserva>(entity =>
        {
            entity.HasKey(e => e.CodigoReserva).HasName("PK__Factura___3E81AB3EB7921E64");

            entity.Property(e => e.CodigoReserva).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.CodigoMesaNavigation).WithMany(p => p.FacturaReservas)
                .HasPrincipalKey(p => p.Codigo)
                .HasForeignKey(d => d.CodigoMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaReserva_Mesa");

            entity.HasOne(d => d.CodigoReservaNavigation).WithOne(p => p.FacturaReserva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaReserva_Reserva");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.FacturaReservas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaReserva_MetodoPago");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasOne(d => d.CedulaNavigation).WithMany(p => p.Inventarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Usuario");

            entity.HasOne(d => d.CodigoMenuNavigation).WithMany(p => p.Inventarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventario_Menu");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Menu__06370DADFA638016");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasOne(d => d.CodigoReservaNavigation).WithMany(p => p.Mesas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mesa_Reserva");

            entity.HasOne(d => d.Disponibilidad).WithMany(p => p.Mesas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mesa_Disponibilidad");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.MetodoPagoId).HasName("PK__Metodo_p__1BF38F8488F8A238");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Reserva__06370DAD6FC7F461");

            entity.HasOne(d => d.CedulaNavigation).WithMany(p => p.Reservas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_Usuario");

            entity.HasOne(d => d.TipoReserva).WithMany(p => p.Reservas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_TipoReserva");
        });

        modelBuilder.Entity<TipoReserva>(entity =>
        {
            entity.HasKey(e => e.TipoReservaId).HasName("PK__Tipo_res__66B7C30240186152");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.TipoUsuarioId).HasName("PK__Tipo_usu__E6E6B2D9DB419760");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__Usuario__B4ADFE392C628B76");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.TipoUsuario).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_TipoUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
