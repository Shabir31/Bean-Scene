using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("Sitting")]
    [Authorize(Roles = "Manager")]
    public class SittingController : AdministrationController
    {
        
        public SittingController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("View")]
        public IActionResult ViewSitting()
        {
            var sittings = _context.Sittings
                .Include(s => s.Restaurant)
                .Include(s => s.SittingType)
                .ToList();
            return View(sittings);
        }


        [HttpGet, Route("Add")]
        public IActionResult AddSitting()
        {
            var sittings = _context.Sittings
                .Include(s => s.Restaurant)
                .Include(s => s.SittingType)
                .ToList();

            var restaurant = _context.Restaurants.ToList();
            ViewBag.RestaurantVB = new SelectList(restaurant, "Id", "RestaurantName");
            var sittingType = _context.SittingTypes.ToList();
            ViewBag.SittingTypeVB = new SelectList(sittingType, "Id", "SittingTypeName");
            return View();
        }
        [HttpPost, Route("Add")]
        public async Task<IActionResult> AddSitting(Sitting s)
        {
            var sittings = _context.Sittings
                .Include(s => s.Restaurant)
                .Include(s => s.SittingType)
                .ToList();

            var newSitting = new Sitting
            {
                SittingName = s.SittingName,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Capacity = s.Capacity,
                Active = s.Active,
                RestaurantId = s.RestaurantId,
                SittingTypeId = s.SittingTypeId
            };

            _context.Sittings.Add(newSitting);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewSitting");

        }


        [HttpGet, Route("{id}/Edit")]
        public IActionResult EditSitting(int id)
        {
            var sittings = _context.Sittings.Find(id);

            if (sittings != null)
            {
                var editSitting = new Sitting()
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
                var restaurant = _context.Restaurants.ToList();
                ViewBag.RestaurantVB = new SelectList(restaurant, "Id", "RestaurantName");
                var sittingTypes = _context.SittingTypes.ToList();
                ViewBag.SittingTypeVB = new SelectList(sittingTypes, "Id", "SittingTypeName");
                return View(editSitting);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewSitting");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> EditSitting(Sitting sitting)
        //{

        //        var sittings = new Sitting()
        //        {
        //            Id = sitting.Id,
        //            SittingName = sitting.SittingName,
        //            StartTime = sitting.StartTime,
        //            EndTime = sitting.EndTime,
        //            Capacity = sitting.Capacity,
        //            Active = sitting.Active,
        //            RestaurantId = sitting.RestaurantId,
        //            SittingTypeId = sitting.SittingTypeId,
        //        };
        //        _context.Sittings.Update(sittings);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("ViewSitting");

        //}
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditSitting(Sitting sitting)
        {


            var editSitting = _context.Sittings
                .Where(s => s.Id == sitting.Id)
                .FirstOrDefault();

            editSitting.Id = sitting.Id;
            editSitting.SittingName = sitting.SittingName;
            editSitting.StartTime = sitting.StartTime;
            editSitting.EndTime = sitting.EndTime;
            editSitting.Capacity = sitting.Capacity;
            editSitting.Active = sitting.Active;
            editSitting.RestaurantId = sitting.RestaurantId;
            editSitting.SittingTypeId = sitting.SittingTypeId;
            _context.Sittings.Update(editSitting);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewSitting");

        }


        [HttpGet, Route("{id}/Delete")]
        public IActionResult DeleteSitting(int id)
        {
            var sittings = _context.Sittings.Find(id);

            if (sittings != null)
            {
                var deleteSitting = new Sitting()
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

                return View(deleteSitting);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("Sitting");
            }
        }
        [HttpPost, Route("{id}/Delete")]
        public async Task<IActionResult> DeleteSitting(Sitting sitting, int id)
        {

            var sittings = _context.Sittings.Find(id);

            _context.Sittings.Remove(sittings);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewSitting");

        }
    }
}
