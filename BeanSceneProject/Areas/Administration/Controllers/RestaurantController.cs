using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("Restaurant")]
    public class RestaurantController : AdministrationController
    {
        public RestaurantController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("View")]
        public IActionResult ViewRestaurant()
        {
            var restaurants = _context.Restaurants
                .ToList();
            return View(restaurants);
        }
        [HttpGet, Route("Add")]
        [Authorize(Roles = "Manager")]
        public IActionResult AddRestaurant()
        {
            var restaurants = _context.Restaurants
                .ToList();

            return View();
        }
        [HttpPost, Route("Add")]
        public async Task<IActionResult> AddRestaurant(Restaurant r)
        {
            var restaurants = _context.Restaurants
                .ToList();

            var newRestaurant = new Restaurant { RestaurantName = r.RestaurantName };

            _context.Restaurants.Add(newRestaurant);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewRestaurant");
        }


        [HttpGet, Route("{id}/Edit")]
        [Authorize(Roles = "Manager")]
        public IActionResult EditRestaurant(int id)
        {
            var restaurant = _context.Restaurants.Find(id);

            if (restaurant != null)
            {
                var editRestaurant = new Restaurant()
                {
                    Id = restaurant.Id,
                    RestaurantName = restaurant.RestaurantName,

                };

                return View(editRestaurant);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewRestaurant");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> EditRestaurant(Restaurant restaurant)
        //{

        //    var editRestaurant = new Restaurant()
        //    {
        //        Id = restaurant.Id,
        //        RestaurantName = restaurant.RestaurantName,

        //    };
        //    _context.Restaurants.Update(editRestaurant);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("ViewRestaurant");

        //}
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditRestaurant(Restaurant restaurant)
        {


            var editRestaurant = _context.Restaurants
                .Where(r => r.Id == restaurant.Id)
                .FirstOrDefault();

            editRestaurant.Id = restaurant.Id;
            editRestaurant.RestaurantName = restaurant.RestaurantName;



            _context.Restaurants.Update(editRestaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewRestaurant");
        }


        [HttpGet, Route("{id}/Delete")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteRestaurant(int id)
        {
            var restaurant = _context.Restaurants.Find(id);

            if (restaurant != null)
            {
                var deleteRestaurant = new Restaurant()
                {
                    Id = restaurant.Id,
                    RestaurantName = restaurant.RestaurantName,

                };

                return View(deleteRestaurant);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewRestaurant");
            }
        }
        [HttpPost, Route("{id}/Delete")]
        public async Task<IActionResult> DeleteRestaurant(Restaurant restaurant, int id)
        {

            var deletRestaurant = _context.Sittings.Find(id);

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewRestaurant");

        }
    }
}
