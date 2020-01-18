using System.Collections.Generic;

namespace ExpenseTracker.Models.Base
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Result = new OperationResult();
        }

        public OperationResult Result { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return Result != null ? Result.IsSuccessful : false;
            }
        }

        public class OperationResult
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
            public List<Error> Errors { get; set; }

            public class Error
            {
                public string ErrorCode { get; set; }
                public string Message { get; set; }
            }
        }
    }
}
