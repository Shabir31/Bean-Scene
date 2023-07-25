using BeanSceneProject.Areas.Administration.Models;
using BeanSceneProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ZstdSharp.Unsafe;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    public class InterReservationController : AdministrationController
    {
        public InterReservationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult ViewInterReservation()
        {
            var getReservation = new ReservationModel();
            getReservation.Reservations = _context.Reservations.ToList();
            getReservation.DiningAreas = _context.DiningAreas.Include(t => t.RestaurantTables).ToList();
            return View(getReservation);
        }

        [HttpGet]
        public IActionResult GetRestaurantTable()
        {
            var getReservation = _context.DiningAreas.Include(t => t.RestaurantTables).ToList();

            var test = getReservation.Select(a => new
            {
                DiningName = a.DiningName,
                DiningId = a.Id,
                Tables = a.RestaurantTables.Select(t => new
                {
                    Id = t.Id,
                    Name = t.SeatName
                })

            });
            //var getReservation = _context.RestaurantTables.Select(r => new
            //{
            //    r.Id,
            //    SeatName = r.SeatName,
            //    r.DiningAreaId
            //}).ToList();
            return Json(test);
        }


        [HttpPost]
        public JsonResult AddRestaurantTable(TableModel table)
        {
            var restaurantTable = new RestaurantTable()
            {
                Id = table.Id,
                SeatName = table.SeatName,
                DiningAreaId = table.DiningAreaId,

            };
            _context.RestaurantTables.Add(restaurantTable);
            _context.SaveChanges();
            return new JsonResult("table Added");
        }

        public class StuffToMap
        {
            public string StatusToSet { get; set; }
        }


        [HttpPost]
        public JsonResult EditStatus(int id, [FromBody] StuffToMap statusToSet)
        {
            var reservation = _context.Reservations.Find(id);

            var reservationStatus = _context.ReservationsStatuses
                .Where((s) => s.ReservationStatusName.ToLower() == statusToSet.StatusToSet.ToLower())
                .FirstOrDefault();

            if (reservation != null)
            {
                reservation.ReservationStatus = reservationStatus;

                _context.SaveChanges();
                return new JsonResult(reservationStatus);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
            };
            
            return new JsonResult("Status Changed");
        }

    }
}
