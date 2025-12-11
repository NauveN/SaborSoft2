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
    public class DetailsModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public DetailsModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        public Inventario Inventario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios.FirstOrDefaultAsync(m => m.Codigo == id);
            if (inventario == null)
            {
                return NotFound();
            }
            else
            {
                Inventario = inventario;
            }
            return Page();
        }
    }
}
