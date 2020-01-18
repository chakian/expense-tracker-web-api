using ExpenseTracker.Models.Base;
using System.Collections.Generic;

namespace ExpenseTracker.Business.Extensions
{
    public static class ResponseExtension
    {
        public static void AddError(this BaseResponse response, string errorCode, string message = "")
        {
            if (response.Result == null) response.Result = new BaseResponse.OperationResult();
            if (response.Result.Errors == null) response.Result.Errors = new List<BaseResponse.OperationResult.Error>();

            response.Result.Errors.Add(new BaseResponse.OperationResult.Error()
            {
                ErrorCode = errorCode,
                Message = message
            });
        }

        public static void SetOkResult(this BaseResponse response)
        {
            response.Result = new BaseResponse.OperationResult();
        }
    }
}
