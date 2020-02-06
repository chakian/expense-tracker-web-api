using ExpenseTracker.Common.Interfaces.Models;
using System.Collections.Generic;

namespace ExpenseTracker.Models.Base
{
    public class BaseResponse : IBaseResponse
    {
        public BaseResponse()
        {
            Result = new OperationResult();
        }

        public IOperationResult Result { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return Result != null ? Result.IsSuccessful : false;
            }
        }

        public class OperationResult : IOperationResult
        {
            public bool IsSuccessful
            {
                get
                {
                    if (Errors != null && Errors.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            public List<IError> Errors { get; set; }

            public class Error : IError
            {
                public string ErrorCode { get; set; }
                public string Message { get; set; }
            }
        }
    }
}
