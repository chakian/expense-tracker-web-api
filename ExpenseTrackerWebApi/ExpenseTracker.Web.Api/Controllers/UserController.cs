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
        private readonly CreateUserAndReturnTokenUOW createUserAndReturnToken;
        private readonly AuthenticateUserAndReturnTokenUOW authenticateUserAndReturnToken;

        public UserController(ILogger<UserController> logger, 
            CreateUserAndReturnTokenUOW createUserAndReturnToken,
            AuthenticateUserAndReturnTokenUOW authenticateUserAndReturnToken)
            : base(logger)
        {
            this.createUserAndReturnToken = createUserAndReturnToken;
            this.authenticateUserAndReturnToken = authenticateUserAndReturnToken;
        }

        [HttpPost]
        [Route("authenticate")]
        public ActionResult Authenticate(AuthenticateUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = authenticateUserAndReturnToken.Execute(model);

            return GetActionResult(response);
        }

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

            var response = createUserAndReturnToken.Execute(model);

            return GetActionResult(response);
        }
    }
}
