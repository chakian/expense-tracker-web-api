using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Models.Base;
using ExpenseTracker.Models.UserModels;
using ExpenseTracker.UOW.UserWorks;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static ExpenseTracker.Models.Base.BaseResponse.OperationResult;

namespace ExpenseTracker.UOW.Tests.UserTests
{
    public class AuthenticateUserTests : UnitTestBase
    {
        Mock<IUserBusiness> userBusiness;
        Mock<IUserInternalTokenBusiness> userInternalTokenBusiness;
        public AuthenticateUserTests()
        {
            userBusiness = new Mock<IUserBusiness>();
            userInternalTokenBusiness = new Mock<IUserInternalTokenBusiness>();
        }

        private AuthenticateUserAndReturnTokenUOW GetUOW()
        {
            return new AuthenticateUserAndReturnTokenUOW(GetLogger<AuthenticateUserAndReturnTokenUOW>(), GetContext(), userBusiness.Object, userInternalTokenBusiness.Object);
        }

        [Fact]
        public void CreateUserSuccess()
        {
            // Arrange
            AuthenticateUserAndReturnTokenUOW uow = GetUOW();
            AuthenticateUserRequest registerUserRequest = new AuthenticateUserRequest();
            var response = new AuthenticateUserResponse
            {
            };

            userBusiness.Setup(o => o.AuthenticateUser(It.IsAny<AuthenticateUserRequest>())).Returns(Task.FromResult(response));
            userInternalTokenBusiness.Setup(o => o.GenerateToken(It.IsAny<string>(), It.IsAny<string>())).Returns("token");

            // Act
            AuthenticateUserResponse actual = (AuthenticateUserResponse)uow.Execute(registerUserRequest);

            // Assert
            AssertSuccessCase(actual);
            Assert.False(string.IsNullOrEmpty(actual.Token));
        }

        [Fact]
        public void CreateUserFailAuthenticate()
        {
            // Arrange
            AuthenticateUserAndReturnTokenUOW uow = GetUOW();
            AuthenticateUserRequest registerUserRequest = new AuthenticateUserRequest();
            var response = new AuthenticateUserResponse
            {
                Token = "test",
                Result = new BaseResponse.OperationResult()
                {
                    Errors = new List<IError>()
                    {
                        new Error(){ErrorCode="", Message=""}
                    }
                }
            };

            userBusiness.Setup(o => o.AuthenticateUser(It.IsAny<AuthenticateUserRequest>())).Returns(Task.FromResult(response));

            // Act
            AuthenticateUserResponse actual = (AuthenticateUserResponse)uow.Execute(registerUserRequest);

            // Assert
            AssertFailCase(actual);
            Assert.True(string.IsNullOrEmpty(actual.Token));
        }

        [Fact]
        public void CreateUserFailNoToken()
        {
            // Arrange
            AuthenticateUserAndReturnTokenUOW uow = GetUOW();
            AuthenticateUserRequest registerUserRequest = new AuthenticateUserRequest();
            var response = new AuthenticateUserResponse
            {
            };

            userBusiness.Setup(o => o.AuthenticateUser(It.IsAny<AuthenticateUserRequest>())).Returns(Task.FromResult(response));
            userInternalTokenBusiness.Setup(o => o.GenerateToken(It.IsAny<string>(), It.IsAny<string>())).Returns("");

            // Act
            AuthenticateUserResponse actual = (AuthenticateUserResponse)uow.Execute(registerUserRequest);

            // Assert
            AssertFailCase(actual);
            Assert.True(string.IsNullOrEmpty(actual.Token));
        }

        [Fact]
        public void ExceptionCase()
        {
            // Arrange
            AuthenticateUserAndReturnTokenUOW uow = GetUOW();
            AuthenticateUserRequest registerUserRequest = new AuthenticateUserRequest();
            var response = new AuthenticateUserResponse
            {
            };

            userBusiness.Setup(o => o.AuthenticateUser(It.IsAny<AuthenticateUserRequest>())).Throws(new System.Exception());

            // Act
            BaseResponse actual = (BaseResponse)uow.Execute(registerUserRequest);

            // Assert
            AssertFailCase(actual);
        }
    }
}
