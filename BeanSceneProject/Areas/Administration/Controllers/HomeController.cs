using BeanSceneProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BeanSceneProject.Areas.Membership.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using NuGet.Versioning;
using Microsoft.AspNetCore.Identity;
using BeanSceneProject.Models;
using System.Data;
using System.Linq.Expressions;
using System.Diagnostics.Metrics;
using System.Runtime.Serialization;
//using AspNetCore;

namespace BeanSceneProject.Areas.Administration.Controllers
{
    public class HomeController : AdministrationController
    {
        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, roleManager)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }


    }
}
