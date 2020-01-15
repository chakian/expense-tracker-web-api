using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Models.UserModels;
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
        private readonly IUserBusiness userBusiness;
        private readonly IUserInternalTokenBusiness userInternalTokenBusiness;

        public UserController(ILogger<UserController> logger, IUserBusiness userBusiness, IUserInternalTokenBusiness userInternalTokenBusiness)
            : base(logger)
        {
            this.userBusiness = userBusiness;
            this.userInternalTokenBusiness = userInternalTokenBusiness;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult> Authenticate(AuthenticateUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authenticationResponse = await userBusiness.AuthenticateUser(model);

            if (authenticationResponse == null)
                return StatusCode(500);

            if (authenticationResponse.Result.IsSuccessful)
            {
                return Ok(authenticationResponse);
            }
            else
            {
                return StatusCode(500, authenticationResponse.Result);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!model.Password.Equals(model.PasswordRepeat))
            {
                return BadRequest("Passwords do not match");
            }

            var response = await userBusiness.RegisterUser(model);

            if (response == null)
                return StatusCode(500);

            if (response.Result.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response.Result);
            }
        }
    }
}
