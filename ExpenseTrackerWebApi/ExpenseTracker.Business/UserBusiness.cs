﻿using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Extensions;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Business.Options;
using ExpenseTracker.Common.Constants;
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

        private User GetUserByEmail(string email)
        {
            var user = dbContext.Users.SingleOrDefaultAsync(q => q.Email == email).Result;
            return user;
        }
        private void SetAuthenticateUserResponseProps(AuthenticateUserResponse response, User user, string requestIp)
        {
            response.Id = user.Id;
            response.Name = user.UserName;
            response.Culture = "";//TODO: Culture

            string token = GenerateToken(response, requestIp).Result;
            response.Token = token;
        }

        public async Task<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest request)
        {
            var user = GetUserByEmail(request.Email);

            AuthenticateUserResponse response = new AuthenticateUserResponse();

            if (user == null)
            {
                response.AddError("Username or password is incorrect");
            }
            else if (user.PasswordHash != EncryptionUtils.GetHash(request.Password))
            {
                response.AddError("Username or password is incorrect");

                user.AccessFailedCount++;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                SetAuthenticateUserResponseProps(response, user, request.RequestIp);

                response.SetOkResult();
            }

            return response;
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            RegisterUserResponse response = new RegisterUserResponse();

            //TODO: Validations
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                response.AddError(ErrorCodes.REGISTER_EMAIL_EMPTY);
            }
            var existingUserWithEmail = GetUserByEmail(request.Email);
            if (existingUserWithEmail != null)
            {
                response.AddError(ErrorCodes.REGISTER_EMAIL_EXISTS);
            }
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                response.AddError(ErrorCodes.REGISTER_PASSWORD_EMPTY);
            }
            if (!request.Password.Equals(request.PasswordRepeat))
            {
                response.AddError(ErrorCodes.REGISTER_PASSWORD_NOT_EQUAL);
            }
            if (request.Password.Length < 6)
            {
                response.AddError(ErrorCodes.REGISTER_PASSWORD_NOT_SAFE);
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                response.AddError(ErrorCodes.REGISTER_NAME_EMPTY);
            }

            if (response.IsSuccessful)
            {
                User newUser = new User()
                {
                    UserName = request.Name,
                    PasswordHash = EncryptionUtils.GetHash(request.Password),
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    IsActive = true,
                    InsertTime = DateTime.Now,
                    Email = request.Email
                };
                dbContext.Users.Add(newUser);
                await dbContext.SaveChangesAsync();

                var user = GetUserByEmail(request.Email);
                SetAuthenticateUserResponseProps(response, user, request.RequestIp);
                response.SetOkResult();
            }

            return response;
        }
    }
}
