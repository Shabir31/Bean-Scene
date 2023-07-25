using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("ReservationType")]
    public class ReservationTypeController : AdministrationController
    {
        public ReservationTypeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("View")]
        public IActionResult ViewReservationType()
        {
            var reservationtype = _context.ReservationTypes
                .ToList();

            return View(reservationtype);
        }


        [HttpGet, Route("Add")]
        [Authorize(Roles = "Manager")]
        public IActionResult AddReservationType()
        {
            var reservationtype = _context.ReservationTypes
                .ToList();

            return View();

        }
        [HttpPost, Route("Add")]
        public async Task<IActionResult> AddReservationType(ReservationType r)
        {
            var reservationType = _context.ReservationTypes
                .ToList();

            var newReservationType = new ReservationType { ReservationTypeName = r.ReservationTypeName };

            _context.ReservationTypes.Add(newReservationType);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewReservationType");
        }


        [HttpGet, Route("{id}/Edit")]
        [Authorize(Roles = "Manager")]
        public IActionResult EditReservationType(int id)
        {
            var reservationtype = _context.ReservationTypes.Find(id);

            if (reservationtype != null)
            {
                var editReservationtype = new ReservationType()
                {
                    Id = reservationtype.Id,
                    ReservationTypeName = reservationtype.ReservationTypeName,

                };

                return View(editReservationtype);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewReservationType");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> EditReservationType(ReservationType reservationtype)
        //{

        //    var editReservationtype = new ReservationType()
        //    {
        //        Id = reservationtype.Id,
        //        ReservationTypeName = reservationtype.ReservationTypeName,

        //    };
        //    _context.ReservationTypes.Update(editReservationtype);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("ViewReservationType");

        //}
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditReservationType(ReservationType reservationtype)
        {


            var editReservationType = _context.ReservationTypes
                .Where(m => m.Id == reservationtype.Id)
                .FirstOrDefault();

            editReservationType.Id = reservationtype.Id;
            editReservationType.ReservationTypeName = reservationtype.ReservationTypeName;


            _context.ReservationTypes.Update(editReservationType);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewReservationType");

        }


        [HttpGet, Route("{id}/Delete")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteReservationType(int id)
        {
            var reservationtype = _context.ReservationTypes.Find(id);

            if (reservationtype != null)
            {
                var deleteReservationtype = new ReservationType()
                {
                    Id = reservationtype.Id,
                    ReservationTypeName = reservationtype.ReservationTypeName,

                };

                return View(deleteReservationtype);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewReservationType");
            }
        }
        [HttpPost, Route("{id}/Delete")]
        public async Task<IActionResult> DeleteReservationType(ReservationType reservationtype, int id)
        {

            var deleteReservationtype = _context.ReservationTypes.Find(id);

            _context.ReservationTypes.Remove(deleteReservationtype);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewReservationType");

        }

    }
}
