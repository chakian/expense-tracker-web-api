using ExpenseTracker.Common.Constants;
using ExpenseTracker.Common.Interfaces.DbContext;
using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Common.Interfaces.UnitOfWork;
using ExpenseTracker.Models.Base;
using Microsoft.Extensions.Logging;
using System;

namespace ExpenseTracker.UOW.Base
{
    public abstract class UnitOfWorkBase<T> : IUnitOfWorkBase
    {
        protected ILogger<T> logger;
        protected IDbContext dbContext;

        public UnitOfWorkBase(ILogger<T> logger, IDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public IBaseResponse Execute(IBaseRequest request)
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

                return new BaseResponse()
                {
                    Result = new BaseResponse.OperationResult()
                    {
                        Errors = new System.Collections.Generic.List<IError>()
                        {
                            new BaseResponse.OperationResult.Error(){
                                ErrorCode=ErrorCodes.GENERIC_ERROR
                            }
                        }
                    }
                };
            }
        }

        internal abstract IBaseResponse ExecuteInternal(IBaseRequest request);
    }
}
