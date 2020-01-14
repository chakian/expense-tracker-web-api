using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Business.Base
{
    public class BusinessBase
    {
        protected BaseResponse.OperationResult GetOkResult(string message = "") //TODO: Maybe a generic success message here?
        {
            BaseResponse.OperationResult result = new BaseResponse.OperationResult()
            {
                IsSuccessful = true,
                ErrorCode = "", //TODO: A successful code maybe... Maybe 0
                Message = message
            };
            return result;
        }

        protected BaseResponse.OperationResult GetErrorResult(string errorCode = "", string message = "") //TODO: Maybe a generic error code and message here?
        {
            BaseResponse.OperationResult result = new BaseResponse.OperationResult()
            {
                IsSuccessful = false,
                ErrorCode = errorCode,
                Message = message
            };
            return result;
        }
    }
}
