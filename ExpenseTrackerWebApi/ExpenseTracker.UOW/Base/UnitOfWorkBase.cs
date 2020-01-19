﻿using ExpenseTracker.Common.Constants;
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
                dbContext.Database.BeginTransaction();
                var response = ExecuteInternal<IBaseRequest, IBaseResponse>(request);
                //TODO: Change this to true
                if (response.IsSuccessful == false)
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

                return new BaseResponse()
                {
                    Result = new BaseResponse.OperationResult()
                    {
                        Errors = new System.Collections.Generic.List<BaseResponse.OperationResult.Error>()
                        {
                            new BaseResponse.OperationResult.Error(){
                                ErrorCode=ErrorCodes.GENERIC_ERROR
                            }
                        }
                    }
                };
            }
        }

        internal abstract V ExecuteInternal<U, V>(U request)
            where U : IBaseRequest
            where V : IBaseResponse;
    }
}