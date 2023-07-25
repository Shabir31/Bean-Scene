using BeanSceneProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    [Route("Settings")]
    public class SettingController : AdministrationController
    {
        public SettingController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet, Route("View")]
        public IActionResult ViewSettings()
        {
            var code = _context.RoleCodes.ToList();

            return View(code);
        }
        [HttpGet, Route("{id}/Edit")]
        public IActionResult EditSettings(int id)
        { 
            var code = _context.RoleCodes.Find(id);

            if (code != null)
            {
                var editCode = new RoleCode()
                {
                    Id = code.Id,
                    ManagerCode = code.ManagerCode,
                    StaffCode = code.StaffCode,
                };
                return View(editCode);
            }
            else
            {
                TempData["errorMessage"] = "Model Data is not valid";
                return RedirectToAction("ViewSettings");
            }

        }
        [HttpPost, Route("{id}/Edit")]
        public async Task<IActionResult> EditSettings(RoleCode roleCode)
        {


            var editCode = _context.RoleCodes
                .Where(r => r.Id == roleCode.Id)
                .FirstOrDefault();

            editCode.Id = roleCode.Id;
            editCode.ManagerCode = roleCode.ManagerCode;
            editCode.StaffCode = roleCode.StaffCode;



            _context.RoleCodes.Update(editCode);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewSettings");
        }
    }
}
