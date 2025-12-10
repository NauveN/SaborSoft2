using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Mesas
{
    public class IndexModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public IndexModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        public IList<Mesa> Mesa { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Mesa = await _context.Mesas
                .Include(m => m.CodigoReservaNavigation)
                .Include(m => m.Disponibilidad).ToListAsync();
        }
    }
}
