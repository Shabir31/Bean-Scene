using BeanSceneProject.Areas.Administration.Models;
using BeanSceneProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    public class CalendarController : AdministrationController
    {
        public CalendarController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult ViewCalendar()
        {

            var m = new ViewCalendarVM();
            m.Restaurants  = _context.Restaurants.ToList();
            m.sittingTypes = _context.SittingTypes.ToList();
     
            return View(m);
        }

        public IActionResult GetEvents()
        {
            var events = _context.Sittings.Select(e => new
            {
                e.Id,
                Title = e.SittingName,
                Start = e.StartTime,
                End = e.EndTime,
                e.Active,
                e.Capacity,
                e.RestaurantId,
                e.Restaurant.RestaurantName,
                e.SittingType.SittingTypeName,
                e.SittingTypeId
            }).ToList();
            return Json(events);
        }

        [HttpPost]
        public JsonResult CreateSitting(ViewCalendarVM sittingVM)
        {
            var sitting = new Sitting()
            {
                Id = sittingVM.Id,
                SittingName = sittingVM.SittingName,
                StartTime = sittingVM.StartTime,
                EndTime = sittingVM.EndTime,
                Capacity = sittingVM.Capacity,
                Active = sittingVM.Active,
                RestaurantId = sittingVM.RestaurantId,
                SittingTypeId = sittingVM.SittingTypeId,
            };
            _context.Sittings.Add(sitting);
            _context.SaveChanges();
            return  new JsonResult("Sitting added to calendar");
        }

        [HttpGet]
        public JsonResult EditSitting(int id)
        {
            var sittings = _context.Sittings.Find(id);
            if(sittings != null)
            {
                var viewcalendar = new ViewCalendarVM()
                {
                    Id = sittings.Id,
                    SittingName = sittings.SittingName,
                    StartTime = sittings.StartTime,
                    EndTime = sittings.EndTime,
                    Capacity = sittings.Capacity,
                    Active = sittings.Active,
                    RestaurantId = sittings.RestaurantId,
                    SittingTypeId = sittings.SittingTypeId,
                };
               
                return new JsonResult(viewcalendar);
            }
            return new JsonResult("Redord is edited");
        }

        [HttpPost]
        public JsonResult UpdateSitting(Sitting sittings)
        {
            _context.Sittings.Update(sittings);
            _context.SaveChanges();
            return new JsonResult("Record is Updated");
        }

        [HttpPost]
        public JsonResult DeleteSitting(int id)
        {
            var data = _context.Sittings.Where(e => e.Id == id).SingleOrDefault();

            if (data != null)
            {
                _context.Sittings.Remove(data);
                _context.SaveChanges();
            }
            else
            {
                _context.SaveChanges();
            }
            return new JsonResult("record deleted");
        }


        [HttpPost]
        public JsonResult SaveEvent(Sitting s)
        {
            if (s.Id > 0)
            {
                var v = _context.Sittings.Where(a => a.Id == s.Id).FirstOrDefault();
                if (v != null)
                {
                    v.SittingName = s.SittingName;
                    v.StartTime = s.StartTime;
                    v.EndTime = s.EndTime;
                    v.Capacity = s.Capacity;
                    v.Active = s.Active;
                    v.RestaurantId = s.RestaurantId;
                    v.SittingTypeId= s.SittingTypeId;
                }
                else
                {
                    _context.Add(s);
                }
                _context.SaveChanges();
            }

            return new JsonResult("record Save");
        }
    }
}
