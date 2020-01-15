﻿using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Business.Options;
using ExpenseTracker.Common.Utils;
using ExpenseTracker.Models.UserModels;
using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Business
{
    public class UserBusiness : BusinessBase<UserBusiness>, IUserBusiness
    {
        private readonly ExpenseTrackerContext dbContext;
        private readonly IUserInternalTokenBusiness userInternalTokenBusiness;
        private readonly JwtOptions appSettings;

        public UserBusiness(ExpenseTrackerContext dbContext, ILogger<UserBusiness> logger, IOptions<JwtOptions> appSettings, IUserInternalTokenBusiness userInternalTokenBusiness)
            : base(logger)
        {
            this.dbContext = dbContext;
            this.appSettings = appSettings.Value;
            this.userInternalTokenBusiness = userInternalTokenBusiness;
        }

        private async Task<string> GenerateToken(AuthenticateUserResponse user, string requestIp)
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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "https://expense.cagdaskorkut.com/api",
                IssuedAt = DateTime.UtcNow
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);

            await userInternalTokenBusiness.WriteToken(tokenString, user.Id, token.Issuer, requestIp, DateTime.UtcNow, token.ValidTo);

            return tokenString;
        }

        public async Task<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest request)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(q => q.Email == request.Email);

            AuthenticateUserResponse response = new AuthenticateUserResponse();

            if (user == null)
            {
                response.Result = GetErrorResult(message: "Username or password is incorrect");
            }
            else if (user.PasswordHash != EncryptionUtils.GetHash(request.Password))
            {
                response.Result = GetErrorResult(message: "Username or password is incorrect");

                user.AccessFailedCount++;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                response.Result = GetOkResult();
                response.Id = user.Id;
                response.Name = user.UserName;
                response.Culture = "";//TODO: Culture

                string token = await GenerateToken(response, request.RequestIp);
                response.Token = token;
            }

            return response;
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest userModel)
        {
            //TODO: Validations
            User user = new User()
            {
                UserName = userModel.Name,
                PasswordHash = EncryptionUtils.GetHash(userModel.Password),
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                IsActive = true,
                InsertTime = DateTime.Now,
                Email = userModel.Email
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            RegisterUserResponse response = (RegisterUserResponse)await AuthenticateUser(new AuthenticateUserRequest()
            {
                Email = userModel.Email,
                Password = EncryptionUtils.GetHash(userModel.Password),
                RequestIp = userModel.RequestIp,
                Culture = userModel.Culture
            });

            string token = await GenerateToken(response, userModel.RequestIp);
            response.Token = token;

            response.Result = GetOkResult();
            return response;
        }
    }
}