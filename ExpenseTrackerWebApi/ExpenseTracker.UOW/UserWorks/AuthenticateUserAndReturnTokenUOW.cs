using ExpenseTracker.Business.Extensions;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Interfaces.DbContext;
using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Models.UserModels;
using ExpenseTracker.UOW.Base;
using Microsoft.Extensions.Logging;
using System;

namespace ExpenseTracker.UOW.UserWorks
{
    public class AuthenticateUserAndReturnTokenUOW : UnitOfWorkBase<AuthenticateUserAndReturnTokenUOW>
    {
        private readonly IUserBusiness userBusiness;
        private readonly IUserInternalTokenBusiness userInternalTokenBusiness;

        public AuthenticateUserAndReturnTokenUOW(ILogger<AuthenticateUserAndReturnTokenUOW> logger, IDbContext dbContext, IUserBusiness userBusiness, IUserInternalTokenBusiness userInternalTokenBusiness)
            : base(logger, dbContext)
        {
            this.userBusiness = userBusiness;
            this.userInternalTokenBusiness = userInternalTokenBusiness;
        }

        internal override IBaseResponse ExecuteInternal(IBaseRequest request)
        {
            AuthenticateUserRequest authenticateUserRequest = (AuthenticateUserRequest)request;
            AuthenticateUserResponse response = userBusiness.AuthenticateUser(authenticateUserRequest).Result;

            if (response.IsSuccessful)
            {
                string token = userInternalTokenBusiness.GenerateToken(response.Id, request.RequestIp);
                if (string.IsNullOrEmpty(token))
                {
                    response.AppendGenericError();
                }
                else
                {
                    userInternalTokenBusiness.WriteToken(token, response.Id, request.RequestIp, "device", DateTime.UtcNow);
                    response.Token = token;
                }
            }

            if (!response.IsSuccessful)
            {
                response.Token = string.Empty;
            }

            return response;
        }
    }
}
