using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Inventarios
{
    public class IndexModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public IndexModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        public IList<Inventario> Inventario { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Inventario = await _context.Inventarios
                .Include(i => i.CedulaNavigation)
                .Include(i => i.CodigoMenuNavigation).ToListAsync();
        }
    }
}
