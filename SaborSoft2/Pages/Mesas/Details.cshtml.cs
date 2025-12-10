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
    public class DetailsModel : PageModel
    {
        private readonly SaborSoft2.Models.SaborCriolloContext _context;

        public DetailsModel(SaborSoft2.Models.SaborCriolloContext context)
        {
            _context = context;
        }

        public Mesa Mesa { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Codigo == id);
            if (mesa == null)
            {
                return NotFound();
            }
            else
            {
                Mesa = mesa;
            }
            return Page();
        }
    }
}
