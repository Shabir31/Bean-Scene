using BeanSceneProject.Data;
using BeanSceneProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BeanSceneProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
            

            //the below is to create Roles for the database

            foreach (var role in new string[] { "Administrator", "Member", "Manager", "Staff" })
            {
                if (!_roleManager.Roles.Any(r => r.Name == role))
                {
                    _roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }

            //the below is to create codes for the first time
            //in an empty database

            foreach (var managerCode in new string[] {"1234"})
            {
                if (!_context.RoleCodes.Any(c=>c.ManagerCode == managerCode))
                {
                    _context.Add(new RoleCode {ManagerCode = managerCode, StaffCode = "5678" });
                }
            }
            _context.SaveChanges();

            foreach (var restaurant in new string[] { "Bean Scene" })
            {
                if (!_context.Restaurants.Any(r => r.RestaurantName == restaurant))
                {
                    _context.Add(new Restaurant { RestaurantName = restaurant });
                }
            }
            _context.SaveChanges();

            foreach (var sittingTypes in new string[] {"Breakfast", "Lunch", "Dinner" })
            {
                if(!_context.SittingTypes.Any(s=>s.SittingTypeName == sittingTypes))
                {
                    _context.Add(new SittingType { SittingTypeName = sittingTypes });
                }
            }
            _context.SaveChanges();

            foreach (var diningAreas in new string[] { "Main", "Outside", "Balcony" })
            {
                if (!_context.DiningAreas.Any(d => d.DiningName == diningAreas))
                {
                    _context.Add(new DiningArea { DiningName = diningAreas, Restaurant = _context.Restaurants.First(r => r.RestaurantName == "Bean Scene") });
                }
            }
            _context.SaveChanges();

            foreach (var mainRestoTables in new string[] { "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M10", })
            {
                if (!_context.RestaurantTables.Any(m => m.SeatName == mainRestoTables))
                {
                    _context.Add(new RestaurantTable { SeatName = mainRestoTables, DiningAreas = _context.DiningAreas.First(d => d.DiningName == "Main") });
                }
            }
            _context.SaveChanges();

            foreach (var outsideRestoTables in new string[] { "O1", "O2", "O3", "O4", "O5", "O6", "O7", "O8", "O9", "O10", })
            {
                if (!_context.RestaurantTables.Any(m => m.SeatName == outsideRestoTables))
                {
                    _context.Add(new RestaurantTable { SeatName = outsideRestoTables, DiningAreas = _context.DiningAreas.First(d => d.DiningName == "Outside") });
                }
            }
            _context.SaveChanges();

            foreach (var balconyRestoTables in new string[] { "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "B10", })
            {
                if (!_context.RestaurantTables.Any(m => m.SeatName == balconyRestoTables))
                {
                    _context.Add(new RestaurantTable { SeatName = balconyRestoTables, DiningAreas = _context.DiningAreas.First(d => d.DiningName == "Balcony") });
                }
            }
            _context.SaveChanges();

            foreach (var resStatus in new string[] { "Pending", "Confirmed", "Seated", "Completed", "Cancelled" })
            {
                if (!_context.ReservationsStatuses.Any(r => r.ReservationStatusName == resStatus))
                {
                    _context.Add(new ReservationStatus { ReservationStatusName = resStatus });
                }
            }
            _context.SaveChanges();

            foreach (var resType in new string[] { "Phone", "Email", "Walk-In", "Website" })
            {
                if (!_context.ReservationTypes.Any(r => r.ReservationTypeName == resType))
                {
                    _context.Add(new ReservationType { ReservationTypeName = resType });
                }
            }
            _context.SaveChanges();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Insta()
        {
            return View();
        }

        public IActionResult TestPage()
        {
            return View();
        }

        [HttpGet]
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

            var reservationStatus = _context.ReservationsStatuses.Where(r=>r.ReservationStatusName == "Pending");
            ViewBag.ReservationStatusVB = new SelectList(reservationStatus, "Id", "ReservationStatusName");

            var reservationType = _context.ReservationTypes.Where(r=>r.ReservationTypeName == "Website");
            ViewBag.ReservationTypeVB = new SelectList(reservationType, "Id", "ReservationTypeName");

            var member = _context.Members.Where(m=>m.FirstName == "Guest");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //This section defines the page that shows
        //up when user logs in based on role
        public IActionResult RedirectUser()
        {
            if (User.IsInRole("Manager"))
            {
                return RedirectToAction("", "Home", new { area = "Administration" });
            }
            else if (User.IsInRole("Staff"))
            {
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
            else if (User.IsInRole("Member"))
            {
                return RedirectToAction("Index", "Home", new { area = "Membership" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
