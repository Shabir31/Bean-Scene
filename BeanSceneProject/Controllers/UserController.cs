using BeanSceneProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BeanSceneProject.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET api/v1/user/me
        [HttpGet("me")]
        public async Task<UserData> GetCurrentUser()
        {
            IdentityUser? user = await _userManager.GetUserAsync(User);

            // Not signed in
            if (user == null)
                return new UserData();

            return new UserData
            {
                Authorized = true,
                Username = user.UserName
            };
        }

    }
}
