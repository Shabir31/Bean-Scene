using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("ReservationStatus")]
    public class ReservationStatusController : AdministrationController
    {
        public ReservationStatusController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("View")]
        public IActionResult ViewReservationStatus()
        {
            var reservationStatus = _context.ReservationsStatuses
                .ToList();

            return View(reservationStatus);
        }


        [HttpGet, Route("Add")]
        [Authorize(Roles = "Manager")]
        public IActionResult AddReservationStatus()
        {
            var reservationStatus = _context.ReservationsStatuses
                .ToList();
            return View();
        }
        [HttpPost, Route("Add")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddReservationStatus(ReservationStatus r)
        {
            var reservationStatus = _context.ReservationsStatuses
                .ToList();

            var newReservationStatus = new ReservationStatus { ReservationStatusName = r.ReservationStatusName };

            _context.ReservationsStatuses.Add(newReservationStatus);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewReservationStatus");
        }


        [HttpGet, Route("{id}/Edit")]
        [Authorize(Roles = "Manager")]
        public IActionResult EditReservationStatus(int id)
        {
            var reservationStatus = _context.ReservationsStatuses.Find(id);

            if (reservationStatus != null)
            {
                var editReservationStatus = new ReservationStatus()
                {
                    Id = reservationStatus.Id,
                    ReservationStatusName = reservationStatus.ReservationStatusName,

                };

                return View(editReservationStatus);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewReservationStatus");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> EditReservationStatus(ReservationStatus reservationStatus)
        //{

        //    var editReservationStatus = new ReservationStatus()
        //    {
        //        Id = reservationStatus.Id,
        //        ReservationStatusName = reservationStatus.ReservationStatusName,

        //    };
        //    _context.ReservationsStatuses.Update(editReservationStatus);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("ViewReservationStatus");

        //}
        [HttpPost, Route("{id}/Edit")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditReservationStatus(ReservationStatus reservationStatus)
        {


            var editReservationStatus = _context.ReservationsStatuses
                .Where(r => r.Id == reservationStatus.Id)
                .FirstOrDefault();

            editReservationStatus.Id = reservationStatus.Id;
            editReservationStatus.ReservationStatusName = reservationStatus.ReservationStatusName;


            _context.ReservationsStatuses.Update(editReservationStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewReservationStatus");

        }


        [HttpGet, Route("{id}/Delete")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteReservationStatus(int id)
        {
            var reservationStatus = _context.ReservationsStatuses.Find(id);

            if (reservationStatus != null)
            {
                var deleteReservationStatus = new ReservationStatus()
                {
                    Id = reservationStatus.Id,
                    ReservationStatusName = reservationStatus.ReservationStatusName,

                };

                return View(deleteReservationStatus);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewReservationStatus");
            }
        }

        [HttpPost, Route("{id}/Delete")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteReservationStatus(ReservationStatus reservationStatus, int id)
        {

            var deleteReservationStatus = _context.ReservationsStatuses.Find(id);

            _context.ReservationsStatuses.Remove(deleteReservationStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewReservationStatus");

        }
    }
}
