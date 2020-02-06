using ExpenseTracker.UOW.UserWorks;
using Xunit;
using ExpenseTracker.Models.UserModels;
using Moq;
using ExpenseTracker.Business.Interfaces;
using System.Threading.Tasks;
using ExpenseTracker.Models.Base;
using System.Collections.Generic;
using ExpenseTracker.Common.Interfaces.Models;
using static ExpenseTracker.Models.Base.BaseResponse.OperationResult;

namespace ExpenseTracker.UOW.Tests.UserTests
{
    public class CreateUserTests : UnitTestBase
    {
        Mock<IUserBusiness> userBusiness;
        Mock<IUserInternalTokenBusiness> userInternalTokenBusiness;
        public CreateUserTests()
        {
            userBusiness = new Mock<IUserBusiness>();
            userInternalTokenBusiness = new Mock<IUserInternalTokenBusiness>();
        }

        private CreateUserAndReturnTokenUOW GetUOW()
        {
            return new CreateUserAndReturnTokenUOW(GetLogger<CreateUserAndReturnTokenUOW>(), GetContext(), userBusiness.Object, userInternalTokenBusiness.Object);
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
                RequestIp = ""
            };
            var response = new CreateUserResponse
            {
                Id = "tretertgre",
                Name = "test",
                Culture = "tr-TR"
            };

            userBusiness.Setup(o => o.CreateUser(It.IsAny<CreateUserRequest>())).Returns(Task.FromResult(response));
            userInternalTokenBusiness.Setup(o => o.GenerateToken(It.IsAny<string>(), It.IsAny<string>())).Returns("token");

            // Act
            CreateUserResponse actual = (CreateUserResponse)uow.Execute(registerUserRequest);

            // Assert
            AssertSuccessCase(actual);
            Assert.False(string.IsNullOrEmpty(actual.Id));
            Assert.False(string.IsNullOrEmpty(actual.Culture));
        }

        [Fact]
        public void CreateUserFail()
        {
            // Arrange
            CreateUserAndReturnTokenUOW uow = GetUOW();
            CreateUserRequest registerUserRequest = new CreateUserRequest();

            var response = new CreateUserResponse
            {
                Token = "dsa",
                Result = new BaseResponse.OperationResult()
                {
                    Errors = new List<IError>()
                    {
                        new Error(){ErrorCode="", Message=""}
                    }
                }
            };

            userBusiness.Setup(o => o.CreateUser(It.IsAny<CreateUserRequest>())).Returns(Task.FromResult(response));

            // Act
            CreateUserResponse actual = (CreateUserResponse)uow.Execute(registerUserRequest);

            // Assert
            AssertFailCase(actual);
            Assert.True(string.IsNullOrEmpty(actual.Token));
        }

        [Fact]
        public void CreateUserFailNoToken()
        {
            // Arrange
            CreateUserAndReturnTokenUOW uow = GetUOW();
            CreateUserRequest registerUserRequest = new CreateUserRequest();

            var response = new CreateUserResponse
            {
            };

            userBusiness.Setup(o => o.CreateUser(It.IsAny<CreateUserRequest>())).Returns(Task.FromResult(response));
            userInternalTokenBusiness.Setup(o => o.GenerateToken(It.IsAny<string>(), It.IsAny<string>())).Returns("");

            // Act
            CreateUserResponse actual = (CreateUserResponse)uow.Execute(registerUserRequest);

            // Assert
            AssertFailCase(actual);
            Assert.True(string.IsNullOrEmpty(actual.Token));
        }
    }
}
