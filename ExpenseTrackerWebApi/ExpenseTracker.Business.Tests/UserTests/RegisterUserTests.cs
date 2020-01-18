using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Models.UserModels;
using Microsoft.Extensions.Options;
using System.Linq;
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
            jwtOptions = Microsoft.Extensions.Options.Options.Create(new Options.JwtOptions() { Secret = "test123456test123456test123456" });
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

        [Fact]
        public void AuthenticateUser_Success()
        {
            // Arrange
            DbContext.Users.Add(new Persistence.Identity.User()
            {
                Email = "test@test.com",
                UserName = "test",
                PasswordHash = "1411501582391102022111941545898146128230134207126393901341752432021821214658220108146"
            });
            DbContext.SaveChanges();
            var expectedUserId = DbContext.Users.Single().Id;

            IUserBusiness userBusiness = new UserBusiness(DbContext, GetLogger<UserBusiness>(), jwtOptions, userInternalTokenBusiness);
            AuthenticateUserRequest authenticateUserRequest = new AuthenticateUserRequest()
            {
                Email = "test@test.com",
                Password = "123456",
                Culture = "",
                RequestIp = "1.1.1.1"
            };

            var expected = new AuthenticateUserResponse()
            {
                Id = expectedUserId,
                Culture = "",
                Name = "test",
                Token = "",
                Result = new Models.Base.BaseResponse.OperationResult()
                {
                    IsSuccessful = true,
                    ErrorCode = "",
                    Message = ""
                }
            };

            // Act
            var actual = userBusiness.AuthenticateUser(authenticateUserRequest).Result;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.NotNull(actual.Token);
            Assert.NotEmpty(actual.Token);
            Assert.Equal(expected.Culture, actual.Culture);

            Assert.NotNull(actual.Result);
            Assert.Equal(expected.Result.IsSuccessful, actual.Result.IsSuccessful);
            Assert.Equal(expected.Result.ErrorCode, actual.Result.ErrorCode);
            Assert.Equal(expected.Result.Message, actual.Result.Message);
        }
    }
}
