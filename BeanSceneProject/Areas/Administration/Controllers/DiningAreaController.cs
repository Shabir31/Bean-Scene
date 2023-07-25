using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("DiningArea")]
    public class DiningAreaController : AdministrationController
    {
        public DiningAreaController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("View")]
        public IActionResult ViewDiningArea()
        {
            var diningArea = _context.DiningAreas
                .Include(s => s.Restaurant)
                .ToList();
            return View(diningArea);
        }


        [HttpGet, Route("Add")]
        [Authorize(Roles = "Manager")]
        public IActionResult AddDiningArea()
        {
            var diningArea = _context.DiningAreas
                .Include(s => s.Restaurant)
                .ToList();

            var restaurant = _context.Restaurants.ToList();
            ViewBag.RestaurantVB = new SelectList(restaurant, "Id", "RestaurantName");

            return View();
        }
        [HttpPost, Route("Add")]
        public async Task<IActionResult> AddDiningArea(DiningArea d)
        {
            var diningArea = _context.DiningAreas
                .Include(s => s.Restaurant)
                .ToList();

            var newDiningArea = new DiningArea { DiningName = d.DiningName, RestaurantId = d.RestaurantId };

            _context.DiningAreas.Add(newDiningArea);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewDiningArea");
        }


        [HttpGet, Route("{id}/Edit")]
        [Authorize(Roles = "Manager")]
        public IActionResult EditDiningArea(int id)
        {
            var diningArea = _context.DiningAreas.Find(id);

            if (diningArea != null)
            {
                var editDiningArea = new DiningArea()
                {
                    Id = diningArea.Id,
                    DiningName = diningArea.DiningName,
                    RestaurantId = diningArea.RestaurantId,

                };
                var restaurant = _context.Restaurants.ToList();
                ViewBag.RestaurantVB = new SelectList(restaurant, "Id", "RestaurantName");

                return View(editDiningArea);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewDiningArea");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> EditDiningArea(DiningArea diningArea)
        //{

        //    var editDiningArea = new DiningArea()
        //    {
        //        Id = diningArea.Id,
        //        DiningName = diningArea.DiningName,
        //        RestaurantId = diningArea.RestaurantId,

        //    };

        //    _context.DiningAreas.Update(editDiningArea);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("ViewDiningArea");

        //}
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditDiningArea(DiningArea diningArea)
        {


            var editDiningArea = _context.DiningAreas
                .Where(r => r.Id == diningArea.Id)
                .FirstOrDefault();

            editDiningArea.Id = diningArea.Id;
            editDiningArea.DiningName = diningArea.DiningName;
            editDiningArea.RestaurantId = diningArea.RestaurantId;



            _context.DiningAreas.Update(editDiningArea);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewDiningArea");
        }


        [HttpGet, Route("{id}/Delete")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteDiningArea(int id)
        {
            var diningArea = _context.DiningAreas.Find(id);

            if (diningArea != null)
            {
                var deleteDiningArea = new DiningArea()
                {
                    Id = diningArea.Id,
                    DiningName = diningArea.DiningName,
                    RestaurantId = diningArea.RestaurantId,

                };

                return View(deleteDiningArea);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewDiningArea");
            }
        }
        [HttpPost, Route("{id}/Delete")]
        public async Task<IActionResult> DeleteDiningArea(DiningArea diningArea, int id)
        {

            var deleteSittingType = _context.Sittings.Find(id);

            _context.DiningAreas.Remove(diningArea);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewDiningArea");

        }

    }
}
