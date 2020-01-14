using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Models.UserModels;
using ExpenseTracker.Web.Api.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/user")]
    public class UserController : ExpenseTrackerControllerBase<UserController>
    {
        private readonly JwtOptions appSettings;
        private readonly IUserBusiness userBusiness;
        private readonly IUserInternalTokenBusiness userInternalTokenBusiness;

        public UserController(ILogger<UserController> logger, IOptions<JwtOptions> appSettings, IUserBusiness userBusiness, IUserInternalTokenBusiness userInternalTokenBusiness)
            : base(logger)
        {
            this.appSettings = appSettings.Value;
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
                // authentication successful so generate jwt token
                string token = await GenerateToken(authenticationResponse);
                authenticationResponse.Token = token;

                return Ok(authenticationResponse);
            }
            else
            {
                return StatusCode(500, authenticationResponse.Result);
            }
        }

        private async Task<string> GenerateToken(AuthenticateUserResponse user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);

            Task.Run(() => userInternalTokenBusiness.WriteToken(tokenString, user.Id, token.Issuer, "", DateTime.UtcNow, token.ValidTo));

            return tokenString;
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
                // authentication successful so generate jwt token
                var token = await GenerateToken(response);
                response.Token = token;

                return Ok(response);
            }
            else
            {
                return StatusCode(500, response.Result);
            }
        }
    }
}
