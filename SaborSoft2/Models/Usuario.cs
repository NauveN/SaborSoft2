using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[Table("Usuario")]
public partial class Usuario
{
    [Key]
    [StringLength(20)]
    public string Cedula { get; set; } = null!;

    [StringLength(80)]
    public string Nombre { get; set; } = null!;

    [Column("Correo_electronico")]
    [StringLength(100)]
    public string CorreoElectronico { get; set; } = null!;

    [Column("Fecha_registro")]
    public DateTime FechaRegistro { get; set; }

    [Column("Tipo_usuario_ID")]
    public int TipoUsuarioId { get; set; }

    [InverseProperty("CedulaNavigation")]
    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    [InverseProperty("CedulaNavigation")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    [ForeignKey("TipoUsuarioId")]
    [InverseProperty("Usuarios")]
    public virtual TipoUsuario TipoUsuario { get; set; } = null!;
}
