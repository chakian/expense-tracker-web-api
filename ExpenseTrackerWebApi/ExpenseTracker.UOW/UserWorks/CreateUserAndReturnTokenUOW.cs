using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Interfaces.DbContext;
using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Models.UserModels;
using ExpenseTracker.UOW.Base;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.UOW.UserWorks
{
    public class CreateUserAndReturnTokenUOW : UnitOfWorkBase<CreateUserAndReturnTokenUOW>
    {
        private readonly IUserBusiness userBusiness;
        private readonly IUserInternalTokenBusiness userInternalTokenBusiness;

        public CreateUserAndReturnTokenUOW(ILogger<CreateUserAndReturnTokenUOW> logger, IDbContext dbContext, IUserBusiness userBusiness, IUserInternalTokenBusiness userInternalTokenBusiness) 
            : base(logger, dbContext)
        {
            this.userBusiness = userBusiness;
            this.userInternalTokenBusiness = userInternalTokenBusiness;
        }

        internal override CreateUserResponse ExecuteInternal(CreateUserRequest request)
        {
            userBusiness.CreateUser(request);
            return null;
        }

        internal override CreateUserResponse ExecuteInternal(CreateUserRequest request) => throw new System.NotImplementedException();
    }
}
