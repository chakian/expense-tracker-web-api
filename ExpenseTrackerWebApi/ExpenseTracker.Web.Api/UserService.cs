using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Web.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Api
{
    public interface IUserService
    {
        Task<UserModel> Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly JwtOptions _appSettings;
        private readonly ExpenseTrackerContext _context;

        public UserService(IOptions<JwtOptions> appSettings, ExpenseTrackerContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public async Task<UserModel> Authenticate(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username && x.PasswordHash == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            UserModel userModel = new UserModel
            {
                Username = user.UserName,
                Token = tokenHandler.WriteToken(token)
            };

            return userModel;
        }
    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
