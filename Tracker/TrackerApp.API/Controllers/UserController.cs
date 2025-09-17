using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TrackerApp.API.Model;
using TrackerApp.API.Services;

namespace TrackerApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }



       
    }
}
