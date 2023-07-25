using BeanSceneProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design.Serialization;
using System.Data;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("SittingType")]
    [Authorize(Roles = "Manager")]
    public class SittingTypeController : AdministrationController
    {
        public SittingTypeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("View")]
        public IActionResult ViewSittingType()
        {
            var sittingType = _context.SittingTypes
                .ToList();
            return View(sittingType);
        }


        [HttpGet, Route("Add")]
        public IActionResult AddSittingType()
        {
            var sittingType = _context.SittingTypes
                .ToList();
            return View();
        }
        [HttpPost, Route("Add")]
        public async Task<IActionResult> AddSittingType(SittingType s)
        {
            var sittingType = _context.SittingTypes
                .ToList();

            var newSittingType = new SittingType
            {
                SittingTypeName = s.SittingTypeName
            };

            _context.SittingTypes.Add(newSittingType);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewSittingType");
        }


        [HttpGet, Route("{id}/Edit")]
        public IActionResult EditSittingType(int id)
        {
            var sittingType = _context.SittingTypes.Find(id);

            if (sittingType != null)
            {
                var editSittingType = new SittingType()
                {
                    Id = sittingType.Id,
                    SittingTypeName = sittingType.SittingTypeName,


                };

                return View(editSittingType);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewSittingType");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> EditSittingType(SittingType sittingType)
        //{

        //    var editSittingType = new SittingType()
        //    {
        //        Id = sittingType.Id,
        //        SittingTypeName = sittingType.SittingTypeName,

        //    };
        //    _context.SittingTypes.Update(editSittingType);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("ViewSittingType");

        //}
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditSittingType(SittingType sittingType)
        {


            var editSittingType = _context.SittingTypes
                .Where(r => r.Id == sittingType.Id)
                .FirstOrDefault();

            editSittingType.Id = sittingType.Id;
            editSittingType.SittingTypeName = sittingType.SittingTypeName;



            _context.SittingTypes.Update(editSittingType);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewSittingType");

        }


        [HttpGet, Route("{id}/Delete")]
        public IActionResult DeleteSittingType(int id)
        {
            var sittingType = _context.SittingTypes.Find(id);

            if (sittingType != null)
            {
                var deleteSittingType = new SittingType()
                {
                    Id = sittingType.Id,
                    SittingTypeName = sittingType.SittingTypeName,


                };

                return View(deleteSittingType);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewSittingType");
            }
        }
        [HttpPost, Route("{id}/Delete")]
        public async Task<IActionResult> DeleteSittingType(SittingType sittingType, int id)
        {

            var deleteSittingType = _context.Sittings.Find(id);

            _context.SittingTypes.Remove(sittingType);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewSittingType");

        }
    }
}
