using ExpenseTracker.Business.Extensions;
using ExpenseTracker.Models.Base;
using Xunit;

namespace ExpenseTracker.Business.Tests.BaseResponseTests
{
    public class ResponseExtensionTests
    {
        [Fact]
        public void AddError_NullCase()
        {
            // Arrange
            BaseResponse response = new BaseResponse();
            response.Result = null;

            // Act
            response.AddError("test");

            //Assert
            Assert.NotNull(response.Result);
            Assert.Single(response.Result.Errors);
            Assert.Equal("test", response.Result.Errors[0].ErrorCode);
        }
    }
}
