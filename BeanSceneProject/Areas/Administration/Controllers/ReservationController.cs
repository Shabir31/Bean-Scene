using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("Reservation")]
    public class ReservationController : AdministrationController
    {
        public ReservationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("View")]
        public IActionResult ViewReservation()
        {
            var reservation = _context.Reservations
                .Include(r => r.Sitting)
                .Include(r => r.ReservationStatus)
                .Include(r => r.ReservationType)
                .Include(r => r.Member)
                .ToList();

            return View(reservation);
        }


        [HttpGet, Route("Add")]
        public IActionResult AddReservation()
        {
            var reservation = _context.Reservations
                .Include(r => r.Sitting)
                .Include(r => r.ReservationStatus)
                .Include(r => r.ReservationType)
                .Include(r => r.Member)
                .ToList();

            var sitting = _context.Sittings.ToList();
            ViewBag.SittingVB = new SelectList(sitting, "Id", "SittingName");

            var reservationStatus = _context.ReservationsStatuses.ToList();
            ViewBag.ReservationStatusVB = new SelectList(reservationStatus, "Id", "ReservationStatusName");

            var reservationType = _context.ReservationTypes.ToList();
            ViewBag.ReservationTypeVB = new SelectList(reservationType, "Id", "ReservationTypeName");

            var member = _context.Members.ToList();
            ViewBag.MemberVB = new SelectList(member, "Id", "FirstName", "LastName");

            return View();
        }
        [HttpPost, Route("Add")]
        public async Task<IActionResult> AddReservation(Reservation r)
        {
            var reservation = _context.Reservations
                .Include(r => r.Sitting)
                .Include(r => r.ReservationStatus)
                .Include(r => r.ReservationType)
                .Include(r => r.Member)
                .ToList();

            var newReservation = new Reservation
            {
                ReservationName = r.ReservationName,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                NoOfGuests = r.NoOfGuests,
                Comments = r.Comments,
                SittingId = r.SittingId,
                ReservationStatusId = r.ReservationStatusId,
                ReservationTypeId = r.ReservationTypeId,
                MemberId = r.MemberId
            };

            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewReservation");
        }


        [HttpGet, Route("{id}/Edit")]
        public IActionResult EditReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);

            if (reservation != null)
            {
                var editReservation = new Reservation()
                {
                    Id = reservation.Id,
                    ReservationName = reservation.ReservationName,
                    StartTime = reservation.StartTime,
                    EndTime = reservation.EndTime,
                    NoOfGuests = reservation.NoOfGuests,
                    Comments = reservation.Comments,
                    SittingId = reservation.SittingId,
                    ReservationStatusId = reservation.ReservationStatusId,
                    ReservationTypeId = reservation.ReservationTypeId,
                    MemberId = reservation.MemberId

                };

                var sitting = _context.Sittings.ToList();
                ViewBag.SittingVB = new SelectList(sitting, "Id", "SittingName");
                var reservationStatus = _context.ReservationsStatuses.ToList();
                ViewBag.ReservationStatusVB = new SelectList(reservationStatus, "Id", "ReservationStatusName");
                var reservationType = _context.ReservationTypes.ToList();
                ViewBag.ReservationTypeVB = new SelectList(reservationType, "Id", "ReservationTypeName");
                var member = _context.Members.ToList();
                ViewBag.MemberVB = new SelectList(member, "Id", "FirstName", "LastName");

                return View(editReservation);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewRestaurantTable");
            }
        }
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditReservation(Reservation reservation)
        {


            var editReservation = _context.Reservations
                .Where(r => r.Id == reservation.Id)
                .First();

            editReservation.Id = reservation.Id;
            editReservation.ReservationName = reservation.ReservationName;
            editReservation.StartTime = reservation.StartTime;
            editReservation.EndTime = reservation.EndTime;
            editReservation.NoOfGuests = reservation.NoOfGuests;
            editReservation.Comments = reservation.Comments;
            editReservation.SittingId = reservation.SittingId;
            editReservation.ReservationStatusId = reservation.ReservationStatusId;
            editReservation.ReservationTypeId = reservation.ReservationTypeId;
            editReservation.MemberId = reservation.MemberId;


            _context.Reservations.Update(editReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewReservation");
        }


        [HttpGet, Route("{id}/Delete")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteReservation(int id)
        {
            var reservations = _context.Reservations.Find(id);

            if (reservations != null)
            {
                var deleteReservation = new Reservation()
                {
                    Id = reservations.Id,
                    ReservationName = reservations.ReservationName,
                    StartTime = reservations.StartTime,
                    EndTime = reservations.EndTime,
                    NoOfGuests = reservations.NoOfGuests,
                    Comments = reservations.Comments,
                    SittingId = reservations.SittingId,
                    ReservationStatusId = reservations.ReservationTypeId,
                    ReservationTypeId = reservations.ReservationTypeId,
                    MemberId = reservations.MemberId
                };

                return View(deleteReservation);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("Sitting");
            }
        }
        [HttpPost, Route("{id}/Delete")]
        public async Task<IActionResult> DeleteReservation(Reservation reservation, int id)
        {

            var deleteReservations = _context.Reservations.Find(id);

            _context.Reservations.Remove(deleteReservations);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewReservation");

        }

    }
}
