using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[Table("Tipo_reserva")]
public partial class TipoReserva
{
    [Key]
    [Column("Tipo_reserva_ID")]
    public int TipoReservaId { get; set; }

    [Column("Tipo_reserva")]
    [StringLength(50)]
    public string TipoReserva1 { get; set; } = null!;

    [InverseProperty("TipoReserva")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
