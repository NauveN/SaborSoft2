using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[PrimaryKey("Codigo", "CodigoReserva")]
[Table("Mesa")]
[Index("Codigo", Name = "UQ_Mesa_Codigo", IsUnique = true)]
public partial class Mesa
{
    [Key]
    public int Codigo { get; set; }

    [Key]
    [Column("Codigo_reserva")]
    public int CodigoReserva { get; set; }

    public int Cantidad { get; set; }

    [Column("Disponibilidad_ID")]
    public int DisponibilidadId { get; set; }

    [ForeignKey("CodigoReserva")]
    [InverseProperty("Mesas")]
    public virtual Reserva CodigoReservaNavigation { get; set; } = null!;

    [ForeignKey("DisponibilidadId")]
    [InverseProperty("Mesas")]
    public virtual Disponibilidad Disponibilidad { get; set; } = null!;

    [InverseProperty("CodigoMesaNavigation")]
    public virtual ICollection<FacturaReserva> FacturaReservas { get; set; } = new List<FacturaReserva>();
}
