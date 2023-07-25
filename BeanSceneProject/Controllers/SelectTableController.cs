using BeanSceneProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeanSceneProject.Controllers
{
    [Route("api/v1/selecttable")]
    [ApiController]
    public class SelectTableController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public SelectTableController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantTable>>> GetTables()
        {
            var restaurantTable = _context.RestaurantTables
                .ToList();
            {
                var tables = await _context.RestaurantTables.ToListAsync();
                return restaurantTable;
            }

        }

    }
}
