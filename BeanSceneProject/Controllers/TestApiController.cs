using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BeanSceneProject.Controllers
{
    [Route("api/v1/test")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        [HttpGet("roles")]
        public IActionResult Roles()
        {
            HashSet<string> roles = new();

            foreach (string role in User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value))
            {
                roles.Add(role);
            }

            return Ok(roles);
        }

        [HttpGet("authorize")]
        [Authorize]
        public IActionResult Authorize()
        {
            return Ok();
        }

        [HttpGet("authorize-admin")]
        [Authorize(Roles = "Manager")]
        public IActionResult AuthorizeAdmin()
        {
            return Ok();
        }

        [HttpGet("authorize-user")]
        [Authorize(Roles = "Member")]
        public IActionResult AuthorizeUser()
        {
            return Ok();
        }
    }
}

