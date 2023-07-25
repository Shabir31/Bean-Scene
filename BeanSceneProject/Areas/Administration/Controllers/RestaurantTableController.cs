using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("RestaurantTable")]
    [Authorize(Roles = "Manager")]
    public class RestaurantTableController : AdministrationController
    {
        public RestaurantTableController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("View")]
        public IActionResult ViewRestaurantTable()
        {
            var restaurantTable = _context.RestaurantTables
                .Include(d => d.DiningAreas)
                .ToList();
            return View(restaurantTable);
        }


        [HttpGet, Route("Add")]
        public IActionResult AddRestaurantTable()
        {
            var restaurantTable = _context.RestaurantTables
                .Include(d => d.DiningAreas)
                .ToList();

            var diningArea = _context.DiningAreas.ToList();
            ViewBag.DiningAreaVB = new SelectList(diningArea, "Id", "DiningName");

            return View();
        }
        [HttpPost, Route("Add")]
        public async Task<IActionResult> AddRestaurantTable(RestaurantTable r)
        {
            var restaurantTable = _context.RestaurantTables
                .Include(d => d.DiningAreas)
                .ToList();

            var newRestaurantTable = new RestaurantTable { SeatName = r.SeatName, DiningAreaId = r.DiningAreaId };

            _context.RestaurantTables.Add(newRestaurantTable);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewRestaurantTable");
        }


        [HttpGet, Route("{id}/Edit")]
        public IActionResult EditRestaurantTable(int id)
        {
            var restaurantTable = _context.RestaurantTables.Find(id);

            if (restaurantTable != null)
            {
                var editRestaurantTable = new RestaurantTable()
                {
                    Id = restaurantTable.Id,
                    SeatName = restaurantTable.SeatName,
                    DiningAreaId = restaurantTable.DiningAreaId,

                };

                var diningArea = _context.DiningAreas.ToList();
                ViewBag.DiningAreaVB = new SelectList(diningArea, "Id", "DiningName");

                return View(editRestaurantTable);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewRestaurantTable");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> EditRestaurantTable(RestaurantTable restaurantTable)
        //{

        //    var editRestaurantTable = new RestaurantTable()
        //    {
        //        Id = restaurantTable.Id,
        //        SeatName = restaurantTable.SeatName,
        //        DiningAreaId = restaurantTable.DiningAreaId,

        //    };

        //    _context.RestaurantTables.Update(editRestaurantTable);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("ViewRestaurantTable");

        //}
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditRestaurantTable(RestaurantTable restaurantTable)
        {


            var editRestaurantTable = _context.RestaurantTables
                .Where(r => r.Id == restaurantTable.Id)
                .FirstOrDefault();

            editRestaurantTable.Id = restaurantTable.Id;
            editRestaurantTable.SeatName = restaurantTable.SeatName;
            editRestaurantTable.DiningAreaId = restaurantTable.DiningAreaId;



            _context.RestaurantTables.Update(editRestaurantTable);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewRestaurantTable");

        }


        [HttpGet, Route("{id}/Delete")]
        public IActionResult DeleteRestaurantTable(int id)
        {
            var restaurantTable = _context.RestaurantTables.Find(id);

            if (restaurantTable != null)
            {
                var deleteRestaurantTable = new RestaurantTable()
                {
                    Id = restaurantTable.Id,
                    SeatName = restaurantTable.SeatName,
                    DiningAreaId = restaurantTable.DiningAreaId,

                };



                return View(deleteRestaurantTable);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewRestaurantTable");
            }
        }
        [HttpPost, Route("{id}/Delete")]
        public async Task<IActionResult> DeleteRestaurantTable(RestaurantTable restaurantTable, int id)
        {

            var deleteRestaurantTable = _context.RestaurantTables.Find(id);

            _context.RestaurantTables.Remove(deleteRestaurantTable);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewRestaurantTable");

        }
    }
}
