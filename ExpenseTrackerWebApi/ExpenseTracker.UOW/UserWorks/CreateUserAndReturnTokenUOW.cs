using ExpenseTracker.Business.Extensions;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Business.Options;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Common.Interfaces.DbContext;
using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Models.UserModels;
using ExpenseTracker.UOW.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace ExpenseTracker.UOW.UserWorks
{
    public class CreateUserAndReturnTokenUOW : UnitOfWorkBase<CreateUserAndReturnTokenUOW>
    {
        private readonly IUserBusiness userBusiness;
        private readonly IUserInternalTokenBusiness userInternalTokenBusiness;
        IOptions<JwtOptions> appSettings;

        public CreateUserAndReturnTokenUOW(ILogger<CreateUserAndReturnTokenUOW> logger, IDbContext dbContext, IUserBusiness userBusiness, IUserInternalTokenBusiness userInternalTokenBusiness, IOptions<JwtOptions> appSettings) 
            : base(logger, dbContext)
        {
            this.userBusiness = userBusiness;
            this.userInternalTokenBusiness = userInternalTokenBusiness;
            this.appSettings = appSettings;
        }

        internal override IBaseResponse ExecuteInternal(IBaseRequest request)
        {
            CreateUserRequest createUserRequest = (CreateUserRequest)request;

            CreateUserResponse response = userBusiness.CreateUser(createUserRequest).Result;
            
            string token = userInternalTokenBusiness.GenerateToken(response.Id, request.RequestIp);
            response.Token = token;
            
            var writeTokenResponse = userInternalTokenBusiness.WriteToken(token, response.Id, appSettings.Value.Issuer, request.RequestIp, DateTime.Now, DateTime.Now.AddDays(appSettings.Value.ValidDays));
            if (!writeTokenResponse.IsCompletedSuccessfully)
            {
                response.AppendError(new Models.Base.BaseResponse.OperationResult.Error() { ErrorCode = ErrorCodes.GENERIC_ERROR });
            }
            
            return response;
        }
    }
}
