using ExpenseTracker.UOW.UserWorks;
using ExpenseTracker.Business.Options;
using Xunit.Abstractions;
using ExpenseTracker.Business;
using Xunit;
using ExpenseTracker.Models.UserModels;
using Moq;

namespace ExpenseTracker.UOW.Tests.UserTests
{
    public class CreateUserTests : UnitTestBase
    {
        Mock<UserBusiness> userBusiness;
        Mock<UserInternalTokenBusiness> userInternalTokenBusiness;
        public CreateUserTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            userBusiness = new Mock<UserBusiness>();
            userInternalTokenBusiness = new Mock<UserInternalTokenBusiness>();
        }

        private CreateUserAndReturnTokenUOW GetUOW()
        {
            Microsoft.Extensions.Options.IOptions<JwtOptions> jwtOptions = Microsoft.Extensions.Options.Options.Create(new JwtOptions() { Secret = "test123456test123456test123456", Issuer = "issuer", ValidDays = 10 });
            return new CreateUserAndReturnTokenUOW(GetLogger<CreateUserAndReturnTokenUOW>(), DbContext, userBusiness.Object, userInternalTokenBusiness.Object, jwtOptions);
        }

        [Fact]
        public void CreateUserSuccess()
        {
            // Arrange
            CreateUserAndReturnTokenUOW uow = GetUOW();
            CreateUserRequest registerUserRequest = new CreateUserRequest()
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
                Culture = null
            };

            //userBusiness.Setup CreateUser

            // Act
            CreateUserResponse actual = (CreateUserResponse)uow.Execute(registerUserRequest);

            // Assert
            AssertSuccessCase(actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Culture, actual.Culture);
        }
    }
}
