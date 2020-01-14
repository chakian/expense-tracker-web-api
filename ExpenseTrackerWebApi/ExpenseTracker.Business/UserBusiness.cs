using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Utils;
using ExpenseTracker.Models.UserModels;
using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ExpenseTracker.Business
{
    public class UserBusiness : BusinessBase, IUserBusiness
    {
        private readonly ExpenseTrackerContext dbContext;
        private readonly IUserInternalTokenBusiness userInternalTokenBusiness;

        public UserBusiness(ExpenseTrackerContext dbContext, IUserInternalTokenBusiness userInternalTokenBusiness)
        {
            this.dbContext = dbContext;
            this.userInternalTokenBusiness = userInternalTokenBusiness;
        }

        public async Task<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest request)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(q => q.Email == request.Email && q.PasswordHash == EncryptionUtils.GetHash(request.Password));

            AuthenticateUserResponse response = new AuthenticateUserResponse();

            if (user == null)
            {
                response.Result = GetErrorResult(message: "Username or password is incorrect");
            }
            else
            {
                response.Result = GetOkResult();
                response.Id = user.Id;
                response.Name = user.UserName;
                response.Culture = "";//TODO: Culture
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
            response.Result = GetOkResult();
            return response;
        }
    }
}
