using ExpenseTracker.Web.Api.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/user")]
    public class UserController : ExpenseTrackerControllerBase<UserController>
    {
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
            : base(logger)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.Authenticate(model.Username, model.Password);
            
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.Register(model);

            return Ok();
        }
    }
}
