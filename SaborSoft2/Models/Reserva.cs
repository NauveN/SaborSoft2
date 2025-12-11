using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[Table("Reserva")]
[Index("Cedula", "Fecha", Name = "UQ_Reserva_Cedula_Fecha", IsUnique = true)]
[Index("CodigoUnico", Name = "UQ_Reserva_Codigo_unico", IsUnique = true)]
public partial class Reserva
{
    [Key]
    public int Codigo { get; set; }

    [StringLength(20)]
    public string Cedula { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public int Cantidad { get; set; }

    [Column("Tipo_reserva_ID")]
    public int TipoReservaId { get; set; }

    [Column("Codigo_unico")]
    [StringLength(6)]
    public string CodigoUnico { get; set; } = null!;

    [ForeignKey("Cedula")]
    [InverseProperty("Reservas")]
    public virtual Usuario CedulaNavigation { get; set; } = null!;

    [InverseProperty("CodigoReservaNavigation")]
    public virtual FacturaReserva? FacturaReserva { get; set; }

    [InverseProperty("CodigoReservaNavigation")]
    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();

    [ForeignKey("TipoReservaId")]
    [InverseProperty("Reservas")]
    public virtual TipoReserva TipoReserva { get; set; } = null!;
}
