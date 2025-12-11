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
        private readonly SaborCriolloContext _context;

        public IndexModel(SaborCriolloContext context)
        {
            _context = context;
        }

        public IList<Usuario> Usuarios { get; set; } = new List<Usuario>();

        [BindProperty(SupportsGet = true)]
        public string? SearchCedula { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchNombre { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Usuarios
                .Include(u => u.TipoUsuario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchCedula))
            {
                query = query.Where(u => u.Cedula.Contains(SearchCedula));
            }

            if (!string.IsNullOrWhiteSpace(SearchNombre))
            {
                query = query.Where(u => u.Nombre.Contains(SearchNombre));
            }

            Usuarios = await query
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }
    }
}
