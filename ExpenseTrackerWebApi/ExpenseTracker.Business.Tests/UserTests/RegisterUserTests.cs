using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Constants;
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
        public RegisterUserTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
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
                Email = "test@test.com",
                Name = "test",
                Password = "123456",
                PasswordRepeat = "123456",
                Culture = "",
                RequestIp = ""
            };
            var expected = new AuthenticateUserResponse()
            {
                Name = "test",
                Culture = ""
            };

            // Act
            var actual = userBusiness.RegisterUser(registerUserRequest).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.NotNull(actual.Token);
            Assert.NotEmpty(actual.Token);
            Assert.Equal(expected.Culture, actual.Culture);
        }

        [Fact]
        public void RegisterUser_Fail_EmailExists()
        {
            // Arrange
            DbContext.Users.Add(new Persistence.Identity.User()
            {
                Email = "test@test.com",
                UserName = "test",
                PasswordHash = "1411501582391102022111941545898146128230134207126393901341752432021821214658220108146"
            });
            DbContext.SaveChanges();

            IUserBusiness userBusiness = new UserBusiness(DbContext, GetLogger<UserBusiness>(), jwtOptions, userInternalTokenBusiness);
            RegisterUserRequest registerUserRequest = new RegisterUserRequest()
            {
                Email = "test@test.com",
                Name = "test",
                Password = "123456",
                PasswordRepeat = "123456",
                Culture = "",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.RegisterUser(registerUserRequest).Result;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Result);
            Assert.False(actual.Result.IsSuccessful);
            Assert.Contains(actual.Result.Errors, q => q.ErrorCode == ErrorCodes.REGISTER_EMAIL_EXISTS);
            Assert.Null(actual.Token);
        }

        [Fact]
        public void RegisterUser_Fail_EmailEmpty()
        {
            // Arrange
            IUserBusiness userBusiness = new UserBusiness(DbContext, GetLogger<UserBusiness>(), jwtOptions, userInternalTokenBusiness);
            RegisterUserRequest registerUserRequest = new RegisterUserRequest()
            {
                Email = "   ",
                Name = "test",
                Password = "123456",
                PasswordRepeat = "123456",
                Culture = "",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.RegisterUser(registerUserRequest).Result;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Result);
            Assert.False(actual.Result.IsSuccessful);
            Assert.Contains(actual.Result.Errors, q => q.ErrorCode == ErrorCodes.REGISTER_EMAIL_EMPTY);
            Assert.Null(actual.Token);
        }

        [Fact]
        public void RegisterUser_Fail_EmailEmpty_AND_NameEmpty()
        {
            // Arrange
            IUserBusiness userBusiness = new UserBusiness(DbContext, GetLogger<UserBusiness>(), jwtOptions, userInternalTokenBusiness);
            RegisterUserRequest registerUserRequest = new RegisterUserRequest()
            {
                Email = "   ",
                Name = " ",
                Password = "123456",
                PasswordRepeat = "123456",
                Culture = "",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.RegisterUser(registerUserRequest).Result;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Result);
            Assert.False(actual.Result.IsSuccessful);
            Assert.Contains(actual.Result.Errors, q => q.ErrorCode == ErrorCodes.REGISTER_EMAIL_EMPTY);
            Assert.Contains(actual.Result.Errors, q => q.ErrorCode == ErrorCodes.REGISTER_NAME_EMPTY);
            Assert.Null(actual.Token);
        }
    }
}
