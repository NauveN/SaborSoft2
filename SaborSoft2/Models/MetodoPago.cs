using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[Table("Metodo_pago")]
public partial class MetodoPago
{
    [Key]
    [Column("Metodo_pago_ID")]
    public int MetodoPagoId { get; set; }

    [Column("Metodo_pago")]
    [StringLength(50)]
    public string MetodoPago1 { get; set; } = null!;

    [InverseProperty("MetodoPago")]
    public virtual ICollection<FacturaReserva> FacturaReservas { get; set; } = new List<FacturaReserva>();
}
