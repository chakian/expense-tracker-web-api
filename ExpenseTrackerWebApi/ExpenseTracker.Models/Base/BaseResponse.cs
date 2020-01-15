namespace ExpenseTracker.Models.Base
{
    public class BaseResponse
    {
        public OperationResult Result { get; set; }

        public class OperationResult
        {
            public bool IsSuccessful { get; set; }
            public string ErrorCode { get; set; }
            public string Message { get; set; }
        }
    }
}
