using BeanSceneProject.Data;
using Microsoft.AspNetCore.Mvc;
using BeanSceneProject.Areas.Administration.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeanSceneProject.Models;

namespace BeanSceneProject.Areas.Membership.Controllers
{
    public class HomeController : MembershipController
    {
        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager) : base(context, userManager, roleManager, signInManager)
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddReservation()
        {
            var isSignedIn = _SignInManager.IsSignedIn(User);

            var reservation = _context.Reservations
                .Include(r => r.Sitting)
                .Include(r => r.ReservationStatus)
                .Include(r => r.ReservationType)
                .Include(r => r.Member)
                .ToList();

            //var name = _context.Members.Where(m => m.FirstName == members.FirstName);

            var sitting = _context.Sittings.ToList();
            ViewBag.SittingVB = new SelectList(sitting, "Id", "SittingName");

            var reservationStatus = _context.ReservationsStatuses.Where(r=>r.ReservationStatusName == "Pending");
            ViewBag.ReservationStatusVB = new SelectList(reservationStatus, "Id", "ReservationStatusName");

            var reservationType = _context.ReservationTypes.Where(r=>r.ReservationTypeName == "Website");
            ViewBag.ReservationTypeVB = new SelectList(reservationType, "Id", "ReservationTypeName");

            var member = _context.Members.Where(m=>m.IdentityUser.Email == User.Identity.Name);
            ViewBag.MemberVB = new SelectList(member, "Id", "FirstName", "LastName");

            return View();
        }
        [HttpPost]
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
                EndTime = r.StartTime.AddMinutes(120),
                NoOfGuests = r.NoOfGuests,
                Comments = r.Comments,
                SittingId = r.SittingId,
                ReservationStatusId = r.ReservationStatusId,
                ReservationTypeId = r.ReservationTypeId,
                MemberId = r.MemberId
            };

            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        public IActionResult UserReservation()
        {
            var isSignedIn = _SignInManager.IsSignedIn(User);

            var member = _context.Members
                .Where(m => m.IdentityUser.Email == User.Identity.Name)
                .First();

            var reservation = _context.Reservations
                .Include(r => r.Sitting)
                .Include(r => r.ReservationStatus)
                .Include(r => r.ReservationType)
                .Include(r => r.Member)
                .Where(m => m.Member.IdentityUser.Email == User.Identity.Name)
                .ToList();

            var reservationMember = new ReservationMemberVM { Reservations = reservation, Member = member };

            return View(reservationMember);
        }
    }
}
