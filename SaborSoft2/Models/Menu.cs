using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[Table("Menu")]
public partial class Menu
{
    [Key]
    public int Codigo { get; set; }

    [StringLength(100)]
    public string Descripcion { get; set; } = null!;

    public int Stock { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Valor { get; set; }

    [InverseProperty("CodigoMenuNavigation")]
    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
