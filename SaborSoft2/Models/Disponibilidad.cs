using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[Table("Disponibilidad")]
public partial class Disponibilidad
{
    [Key]
    [Column("Disponibilidad_ID")]
    public int DisponibilidadId { get; set; }

    [Column("Disponibilidad")]
    [StringLength(100)]
    public string Disponibilidad1 { get; set; } = null!;

    [InverseProperty("Disponibilidad")]
    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
