using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Extensions;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Business.Options;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Models.Base;
using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Persistence.Context.DbModels;
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

        public string GenerateToken(string userId, string requestIp, JwtOptions tokenOptions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(tokenOptions.ValidDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = tokenOptions.Issuer,
                IssuedAt = DateTime.UtcNow
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public string GetUsersActiveToken(string userId, string issuer, string creatingIp)
        {
            var token = GetActiveToken(userId, issuer, creatingIp);
            if (token != null)
            {
                return token.Id;
            }
            else
            {
                return null;
            }
        }

        private UserInternalToken GetActiveToken(string userId, string issuer, string ip, string tokenString = "")
        {
            var token = dbContext.UserInternalTokens.SingleOrDefault(q => q.UserId == userId && q.Issuer == issuer && q.CreatingIp == ip && q.ValidTo > DateTime.UtcNow && q.Id == tokenString);
            if (token == null)
            {
                token = dbContext.UserInternalTokens.SingleOrDefault(q => q.UserId == userId && q.Issuer == issuer && q.CreatingIp == ip && q.ValidTo > DateTime.UtcNow);
            }
            return token;
        }

        public BaseResponse WriteToken(JwtOptions tokenOptions, string tokenString, string userId, string creatingIp, DateTime validFrom, bool isValid = true)
        {
            //TODO: Learn about refresh tokens!
            BaseResponse response = new BaseResponse();
            if (string.IsNullOrEmpty(tokenString))
            {
                response.AddError(ErrorCodes.TOKEN_EMPTY);
            }
            else
            {
                var token = GetActiveToken(userId, tokenOptions.Issuer, creatingIp, tokenString);
                if (token == null)
                {
                    AddNewToken(tokenOptions, tokenString, userId, creatingIp, isValid);
                }
                else if (token.Id != tokenString)
                {
                    token.IsValid = false;
                    token.ValidTo = DateTime.UtcNow;
                    dbContext.SaveChanges();

                    AddNewToken(tokenOptions, tokenString, userId, creatingIp, isValid);
                }
                else
                {
                    token.LastUsedDate = DateTime.UtcNow;
                    token.ValidTo = DateTime.UtcNow.AddDays(tokenOptions.ValidDays);
                    dbContext.SaveChanges();
                }
            }
            return response;
        }
        private void AddNewToken(JwtOptions tokenOptions, string tokenString, string userId, string creatingIp, bool isValid)
        {
            dbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = tokenString,
                UserId = userId,
                Issuer = tokenOptions.Issuer,
                CreatingIp = creatingIp,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(tokenOptions.ValidDays),
                LastUsedDate = DateTime.UtcNow,
                IsValid = isValid
            });
            dbContext.SaveChanges();
        }
    }
}
