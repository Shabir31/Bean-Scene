using BeanSceneProject.Data;
using BeanSceneProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("Member")]
    public class MemberController : AdministrationController
    {
        public MemberController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("View")]
        public async Task<IActionResult> ViewMember()
        {
            var members = _context.Members
                .Include(m => m.IdentityUser)
                .ToList();

            var roles = _roleManager.Roles
                .ToList();

            var membersInRole = new List<MemberVM>();


            foreach (var mem in members)
            {
                var isInRole = await _userManager.IsInRoleAsync(mem.IdentityUser, "Member");
                if (isInRole)
                {
                    var userView = new MemberVM() { Member = mem };
                    userView.Role = roles.FirstOrDefault(r => r.NormalizedName == "MEMBER");
                    membersInRole.Add(userView);
                }
            }


            return View(membersInRole);
        }


        [HttpGet, Route("{id}/Edit")]
        [Authorize(Roles = "Manager")]
        public IActionResult EditMember(int id)
        {
            var viewMember = _context.Members
                .Find(id);

            var viewIdentity = _userManager.Users
                .ToList();

            if (viewMember != null)
            {
                var editMember = new Member { FirstName = viewMember.FirstName, LastName = viewMember.LastName, IdentityUser = viewMember.IdentityUser };

                return View(editMember);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewReservationStatus");
            }
        }
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditMember(Member member)
        {

            //var viewIdentity = _userManager.Users
            //    .ToList();


            var member2Edit = _context.Members
                .Include(m => m.IdentityUser)
                .Where(m => m.Id == member.Id)
                .FirstOrDefault();

            member2Edit.FirstName = member.FirstName;
            member2Edit.LastName = member.LastName;
            member2Edit.IdentityUser.PhoneNumber = member.IdentityUser.PhoneNumber;

            //var viewIdentity2 = viewIdentity.Where(m=>m.Id == member.Id);

            //var editMember = new Member()
            //{
            //    Id = member.Id,
            //    FirstName = member.FirstName,
            //    LastName = member.LastName,
            //    IdentityUser = member.IdentityUser,
            //    IdentityUserId = member.IdentityUserId
            //};
            _context.Members.Update(member2Edit);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewMember");
        }


        [HttpGet, Route("{id}/Delete")]
        [Authorize(Roles = "Manager")]
        public IActionResult DeleteMember(int id)
        {
            var member = _context.Members.Find(id);

            var viewMember = _userManager.Users
                .ToList();

            if (member != null)
            {
                var deleteMember = new Member()
                {
                    Id = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    IdentityUser = member.IdentityUser

                };

                return View(deleteMember);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewReservationStatus");
            }
        }
        [HttpPost, Route("{id}/Delete")]
        public async Task<IActionResult> DeleteMember(Member member, int id)
        {
            var member2Delete = _context.Members
                .Find(id);

            var identity2Delete = _userManager.Users
                .FirstOrDefault(u => u.Id == member2Delete.IdentityUserId);

            _context.Members.Remove(member2Delete);
            await _userManager.DeleteAsync(identity2Delete);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewMember");

        }

    }
}
