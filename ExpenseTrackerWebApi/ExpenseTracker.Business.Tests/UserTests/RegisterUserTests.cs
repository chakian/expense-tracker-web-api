﻿using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Models.UserModels;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.UserTests
{
    public class RegisterUserTests : UnitTestBase
    {
        public RegisterUserTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        private UserBusiness GetUserBusiness()
        {
            return new UserBusiness(GetLogger<UserBusiness>(), DbContext);
        }

        [Fact]
        public void RegisterUserSuccess()
        {
            // Arrange
            IUserBusiness userBusiness = GetUserBusiness();
            CreateUserRequest registerUserRequest = new CreateUserRequest()
            {
                Email = "test@test.com",
                Name = "test",
                Password = "123456",
                PasswordRepeat = "123456",
                RequestIp = ""
            };
            var expected = new AuthenticateUserResponse()
            {
                Name = "test",
                Culture = null
            };

            // Act
            var actual = userBusiness.CreateUser(registerUserRequest).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Culture, actual.Culture);
        }

        [Fact]
        public void RegisterUserFailEmailExists()
        {
            // Arrange
            DbContext.Users.Add(new Persistence.Identity.User()
            {
                Email = "test@test.com",
                UserName = "test",
                PasswordHash = "1411501582391102022111941545898146128230134207126393901341752432021821214658220108146"
            });
            DbContext.SaveChanges();

            IUserBusiness userBusiness = GetUserBusiness();
            CreateUserRequest registerUserRequest = new CreateUserRequest()
            {
                Email = "test@test.com",
                Name = "test",
                Password = "123456",
                PasswordRepeat = "123456",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.CreateUser(registerUserRequest).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.REGISTER_EMAIL_EXISTS);
            Assert.Null(actual.Token);
        }

        [Fact]
        public void RegisterUserFailEmailEmpty()
        {
            // Arrange
            IUserBusiness userBusiness = GetUserBusiness();
            CreateUserRequest registerUserRequest = new CreateUserRequest()
            {
                Email = "   ",
                Name = "test",
                Password = "123456",
                PasswordRepeat = "123456",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.CreateUser(registerUserRequest).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.REGISTER_EMAIL_EMPTY);
            Assert.Null(actual.Token);
        }

        [Fact]
        public void RegisterUserFailEmailEmptyANDNameEmpty()
        {
            // Arrange
            IUserBusiness userBusiness = GetUserBusiness();
            CreateUserRequest registerUserRequest = new CreateUserRequest()
            {
                Email = "   ",
                Name = " ",
                Password = "123456",
                PasswordRepeat = "123456",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.CreateUser(registerUserRequest).Result;

            // Assert
            AssertMultipleErrorCase(actual, ErrorCodes.REGISTER_EMAIL_EMPTY, ErrorCodes.REGISTER_NAME_EMPTY);
            Assert.Null(actual.Token);
        }

        [Fact]
        public void RegisterUserFailPasswordEmpty()
        {
            // Arrange
            IUserBusiness userBusiness = GetUserBusiness();
            CreateUserRequest registerUserRequest = new CreateUserRequest()
            {
                Email = "test@test.com",
                Name = "test",
                Password = "",
                PasswordRepeat = "",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.CreateUser(registerUserRequest).Result;

            // Assert
            AssertMultipleErrorCase(actual, ErrorCodes.REGISTER_PASSWORD_EMPTY, ErrorCodes.REGISTER_PASSWORD_NOT_SAFE);
            Assert.Null(actual.Token);
        }

        [Fact]
        public void RegisterUserFailPasswordNotEqual()
        {
            // Arrange
            IUserBusiness userBusiness = GetUserBusiness();
            CreateUserRequest registerUserRequest = new CreateUserRequest()
            {
                Email = "test@test.com",
                Name = "test",
                Password = "1234567",
                PasswordRepeat = "123456",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.CreateUser(registerUserRequest).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.REGISTER_PASSWORD_NOT_EQUAL);
            Assert.Null(actual.Token);
        }

        [Fact]
        public void RegisterUserFailPasswordNotSafe()
        {
            // Arrange
            IUserBusiness userBusiness = GetUserBusiness();
            CreateUserRequest registerUserRequest = new CreateUserRequest()
            {
                Email = "test@test.com",
                Name = "test",
                Password = "12345",
                PasswordRepeat = "12345",
                RequestIp = ""
            };

            // Act
            var actual = userBusiness.CreateUser(registerUserRequest).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.REGISTER_PASSWORD_NOT_SAFE);
            Assert.Null(actual.Token);
        }
    }
}
