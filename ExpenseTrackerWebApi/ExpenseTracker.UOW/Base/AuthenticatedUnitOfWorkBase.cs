using ExpenseTracker.Common.Constants;
using ExpenseTracker.Common.Interfaces.DbContext;
using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Common.Interfaces.UnitOfWork;
using ExpenseTracker.Models.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ExpenseTracker.UOW.Base
{
    public abstract class AuthenticatedUnitOfWorkBase<T> : IAuthenticatedUnitOfWorkBase
    {
        protected ILogger<T> logger;
        protected IDbContext dbContext;

        public AuthenticatedUnitOfWorkBase(ILogger<T> logger, IDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public IBaseAuthenticatedResponse Execute(IBaseAuthenticatedRequest request)
        {
            try
            {
                // TODO: Transaction mechanism isn't working! Fix this!
                dbContext.Database.BeginTransaction();
                var response = ExecuteInternal(request);
                if (response.IsSuccessful)
                {
                    dbContext.Database.CommitTransaction();
                }
                else
                {
                    dbContext.Database.RollbackTransaction();
                }
                return response;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace);

                dbContext.Database.RollbackTransaction();

                return new BaseAuthenticatedResponse()
                {
                    Result = new BaseResponse.OperationResult()
                    {
                        Errors = new List<IError>()
                        {
                            new BaseResponse.OperationResult.Error(){
                                ErrorCode=ErrorCodes.GENERIC_ERROR
                            }
                        }
                    }
                };
            }
        }

        internal abstract IBaseAuthenticatedResponse ExecuteInternal(IBaseAuthenticatedRequest request);
    }
}
