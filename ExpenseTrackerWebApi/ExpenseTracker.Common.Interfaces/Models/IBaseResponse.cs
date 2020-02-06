using System.Collections.Generic;

namespace ExpenseTracker.Common.Interfaces.Models
{
    public interface IBaseResponse
    {
        bool IsSuccessful { get; }
        IOperationResult Result { get; set; }
    }

    public interface IOperationResult
    {
        bool IsSuccessful { get; }
        List<IError> Errors { get; set; }
    }

    public interface IError
    {
        string ErrorCode { get; set; }
        string Message { get; set; }
    }
}
