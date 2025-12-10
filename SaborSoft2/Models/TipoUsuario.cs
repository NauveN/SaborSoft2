using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SaborSoft2.Models;

[Table("Tipo_usuario")]
public partial class TipoUsuario
{
    [Key]
    [Column("Tipo_usuario_ID")]
    public int TipoUsuarioId { get; set; }

    [Column("Tipo_usuario")]
    [StringLength(50)]
    public string TipoUsuario1 { get; set; } = null!;

    [InverseProperty("TipoUsuario")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
