using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Business.Options;
using ExpenseTracker.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Business
{
    public class UserInternalTokenBusiness : BusinessBase<UserInternalTokenBusiness>, IUserInternalTokenBusiness
    {
        private readonly ExpenseTrackerContext dbContext;
        private readonly JwtOptions appSettings;

        public UserInternalTokenBusiness(ExpenseTrackerContext dbContext, ILogger<UserInternalTokenBusiness> logger, IOptions<JwtOptions> appSettings)
            : base(logger)
        {
            this.dbContext = dbContext;
            this.appSettings = appSettings.Value;
        }

        public string GenerateToken(string userId, string requestIp)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "",
                IssuedAt = DateTime.UtcNow
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);

            WriteToken(tokenString, userId, token.Issuer, requestIp, DateTime.UtcNow, token.ValidTo);

            return tokenString;
        }
        public async Task WriteToken(string tokenString, string userId, string issuer, string creatingIp, DateTime validFrom, DateTime validTo, bool isValid = true)
        {
            //TODO: Learn about refresh tokens!
            var token = await dbContext.UserInternalTokens.SingleOrDefaultAsync(q => q.UserId == userId && q.Issuer == issuer && q.CreatingIp == creatingIp && q.Id == tokenString);
            if (token == null)
            {
                token = await dbContext.UserInternalTokens.SingleOrDefaultAsync(q => q.UserId == userId && q.Issuer == issuer && q.CreatingIp == creatingIp);
                if (token == null)
                {
                    dbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
                    {
                        Id = tokenString,
                        UserId = userId,
                        Issuer = issuer,
                        CreatingIp = creatingIp,
                        ValidFrom = validFrom,
                        ValidTo = validTo,
                        LastUsedDate = DateTime.UtcNow,
                        IsValid = isValid
                    });
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    token.LastUsedDate = DateTime.UtcNow;
                    token.ValidFrom = DateTime.UtcNow.AddDays(60);// TODO: make this a global setting
                    token.Id = tokenString;
                    await dbContext.SaveChangesAsync();
                }
            }
            else
            {
                token.LastUsedDate = DateTime.UtcNow;
                token.ValidFrom = DateTime.UtcNow.AddDays(60);// TODO: make this a global setting
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
