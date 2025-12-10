using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[Table("Factura_reserva")]
public partial class FacturaReserva
{
    [Key]
    [Column("Codigo_reserva")]
    public int CodigoReserva { get; set; }

    [Column("Codigo_mesa")]
    public int CodigoMesa { get; set; }

    [Column("Metodo_pago_ID")]
    public int MetodoPagoId { get; set; }

    public DateTime Fecha { get; set; }

    public int Cantidad { get; set; }

    [ForeignKey("CodigoMesa")]
    [InverseProperty("FacturaReservas")]
    public virtual Mesa CodigoMesaNavigation { get; set; } = null!;

    [ForeignKey("CodigoReserva")]
    [InverseProperty("FacturaReserva")]
    public virtual Reserva CodigoReservaNavigation { get; set; } = null!;

    [ForeignKey("MetodoPagoId")]
    [InverseProperty("FacturaReservas")]
    public virtual MetodoPago MetodoPago { get; set; } = null!;
}
