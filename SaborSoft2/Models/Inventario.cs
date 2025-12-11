using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[PrimaryKey("Codigo", "CodigoMenu")]
[Table("Inventario")]
public partial class Inventario
{
    [Key]
    public int Codigo { get; set; }

    [Key]
    [Column("Codigo_menu")]
    public int CodigoMenu { get; set; }

    [StringLength(20)]
    public string Cedula { get; set; } = null!;

    [StringLength(150)]
    public string? Descripcion { get; set; }

    public int Stock { get; set; }

    [Column("Unidad_medida")]
    [StringLength(20)]
    public string UnidadMedida { get; set; } = null!;

    [Column("Fecha_adquisicion")]
    public DateOnly FechaAdquisicion { get; set; }

    [Column("Stock_minimo")]
    public int StockMinimo { get; set; }

    [Column("Fecha_actualizacion", TypeName = "datetime")]
    public DateTime? FechaActualizacion { get; set; }

    [ForeignKey("Cedula")]
    [InverseProperty("Inventarios")]
    public virtual Usuario CedulaNavigation { get; set; } = null!;

    [ForeignKey("CodigoMenu")]
    [InverseProperty("Inventarios")]
    public virtual Menu CodigoMenuNavigation { get; set; } = null!;
}
