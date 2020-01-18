using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Models.UserModels;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.UserTests
{
    public class RegisterUserTests : UnitTestBase
    {
        readonly IOptions<Options.JwtOptions> jwtOptions;
        readonly IUserInternalTokenBusiness userInternalTokenBusiness;
        public RegisterUserTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            jwtOptions = Microsoft.Extensions.Options.Options.Create(new Options.JwtOptions() { Secret = "" });
            userInternalTokenBusiness = new UserInternalTokenBusiness(DbContext, GetLogger<UserInternalTokenBusiness>());
        }

        [Fact]
        public void RegisterUser_Success()
        {
            // Arrange
            IUserBusiness userBusiness = new UserBusiness(DbContext, GetLogger<UserBusiness>(), jwtOptions, userInternalTokenBusiness);
            RegisterUserRequest registerUserRequest = new RegisterUserRequest()
            {
                Email = "",
                Name = "",
                Password = "",
                PasswordRepeat = "",
                Culture = "",
                RequestIp = ""
            };
            var expected = new RegisterUserResponse()
            {
                Name = "",
                Token = "",
                Culture = "",
                Result = new Models.Base.BaseResponse.OperationResult()
                {
                    IsSuccessful = true,
                    ErrorCode = "",
                    Message = ""
                }
            };

            // Act
            var actual = userBusiness.RegisterUser(registerUserRequest).Result;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Token, actual.Token);
            Assert.Equal(expected.Culture, actual.Culture);

            Assert.NotNull(actual.Result);
            Assert.Equal(expected.Result.IsSuccessful, actual.Result.IsSuccessful);
            Assert.Equal(expected.Result.ErrorCode, actual.Result.ErrorCode);
            Assert.Equal(expected.Result.Message, actual.Result.Message);
        }
    }
}
