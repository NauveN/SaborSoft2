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
        private readonly SaborCriolloContext _context;

        public IndexModel(SaborCriolloContext context)
        {
            _context = context;
        }

        public IList<Inventario> Inventarios { get; set; } = new List<Inventario>();

        [BindProperty(SupportsGet = true)]
        public string? SearchNombre { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaDesde { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaHasta { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool SoloConStock { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Inventarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchNombre))
            {
                query = query.Where(i =>
                    i.Descripcion.Contains(SearchNombre));
            }

            if (FechaDesde.HasValue)
            {
                var f = DateOnly.FromDateTime(FechaDesde.Value.Date);
                query = query.Where(i => i.FechaAdquisicion >= f);
            }

            if (FechaHasta.HasValue)
            {
                var f = DateOnly.FromDateTime(FechaHasta.Value.Date);
                query = query.Where(i => i.FechaAdquisicion <= f);
            }

            if (SoloConStock)
            {
                query = query.Where(i => i.Stock > 0);
            }

            Inventarios = await query
                .OrderBy(i => i.Descripcion)
                .ToListAsync();
        }
    }
}
