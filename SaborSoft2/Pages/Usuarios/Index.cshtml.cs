using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public IndexModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        public IList<Usuario> Usuario { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Usuario = await _context.Usuarios
                .Include(u => u.TipoUsuario).ToListAsync();
        }
    }
}
