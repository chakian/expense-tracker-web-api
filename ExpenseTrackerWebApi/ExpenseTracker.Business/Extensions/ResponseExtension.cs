using ExpenseTracker.Common.Constants;
using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Language;
using ExpenseTracker.Models.Base;
using System.Collections.Generic;

namespace ExpenseTracker.Business.Extensions
{
    public static class ResponseExtension
    {
        public static void AddError(this BaseResponse response, string errorCode, string message = "")
        {
            if (response.Result == null) response.Result = new BaseResponse.OperationResult();
            if (response.Result.Errors == null) response.Result.Errors = new List<IError>();

            if (string.IsNullOrEmpty(message))
            {
                var resourceText = Resources.ResourceManager.GetString(errorCode);
                if (!string.IsNullOrEmpty(resourceText)) message = resourceText;
            }

            response.Result.Errors.Add(new BaseResponse.OperationResult.Error()
            {
                ErrorCode = errorCode,
                Message = message
            });
        }

        //public static void AppendErrors(this BaseResponse response, List<BaseResponse.OperationResult.Error> errors)
        //{
        //    if (response.Result == null) response.Result = new BaseResponse.OperationResult();
        //    if (response.Result.Errors == null) response.Result.Errors = new List<IError>();

        //    response.Result.Errors.AddRange(errors);
        //}

        public static void AppendError(this BaseResponse response, BaseResponse.OperationResult.Error error)
        {
            if (response.Result == null) response.Result = new BaseResponse.OperationResult();
            if (response.Result.Errors == null) response.Result.Errors = new List<IError>();

            response.Result.Errors.Add(error);
        }

        public static void AppendGenericError(this BaseResponse response)
        {
            response.AppendError(new BaseResponse.OperationResult.Error() { ErrorCode = ErrorCodes.GENERIC_ERROR });
        }

        public static void SetOkResult(this BaseResponse response)
        {
            response.Result = new BaseResponse.OperationResult();
        }
    }
}
