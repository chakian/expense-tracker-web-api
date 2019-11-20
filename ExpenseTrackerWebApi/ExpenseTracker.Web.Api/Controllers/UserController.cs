using ExpenseTracker.Web.Api.Core.Dto.UseCaseRequests;
using ExpenseTracker.Web.Api.Core.Interfaces.UseCases;
using ExpenseTracker.Web.Api.Presenters;
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
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly RegisterUserPresenter _registerUserPresenter;

        public UserController(ILogger<UserController> logger,
            IRegisterUserUseCase registerUserUseCase, 
            RegisterUserPresenter registerUserPresenter)
            : base(logger)
        {
            _registerUserUseCase = registerUserUseCase;
            _registerUserPresenter = registerUserPresenter;
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _registerUserUseCase.Handle(new RegisterUserRequest(request.FirstName, request.LastName, request.Email, request.Username, request.Password), _registerUserPresenter);
            return _registerUserPresenter.ContentResult;
        }
    }
}
