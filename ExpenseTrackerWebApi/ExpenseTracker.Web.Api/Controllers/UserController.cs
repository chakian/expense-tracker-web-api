using ExpenseTracker.Models.UserModels;
using ExpenseTracker.UOW.UserWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Web.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/user")]
    public class UserController : ExpenseTrackerControllerBase<UserController>
    {
        private readonly CreateUserAndReturnTokenUOW createUserAndReturnTokenUOW;

        public UserController(ILogger<UserController> logger, CreateUserAndReturnTokenUOW createUserAndReturnTokenUOW)
            : base(logger)
        {
            this.createUserAndReturnTokenUOW = createUserAndReturnTokenUOW;
        }

        //[HttpPost]
        //[Route("authenticate")]
        //public async Task<ActionResult> Authenticate(AuthenticateUserRequest model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var authenticationResponse = await userBusiness.AuthenticateUser(model);

        //    if (authenticationResponse == null)
        //        return StatusCode(500);

        //    if (authenticationResponse.Result.IsSuccessful)
        //    {
        //        return Ok(authenticationResponse);
        //    }
        //    else
        //    {
        //        return StatusCode(400, authenticationResponse.Result);
        //    }
        //}

        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] CreateUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!model.Password.Equals(model.PasswordRepeat))
            {
                return BadRequest("Passwords do not match");
            }

            var response = createUserAndReturnTokenUOW.Execute(model);

            if (response == null)
                return StatusCode(500);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
