using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaborSoft2.Models;

namespace SaborSoft2.Pages.Inventarios
{
    public class AlertasModel : PageModel
    {
        private readonly SaborCriolloContext _context;

        public AlertasModel(SaborCriolloContext context)
        {
            _context = context;
        }

        public IList<Inventario> IngredientesConAlerta { get; set; } = new List<Inventario>();

        public async Task OnGetAsync()
        {
            IngredientesConAlerta = await _context.Inventarios
                .Where(i => i.Stock <= i.StockMinimo)
                .OrderBy(i => i.Descripcion)
                .ToListAsync();
        }
    }
}
