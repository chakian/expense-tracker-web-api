using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Constants;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.UserTests
{
    public class TokenTests : UnitTestBase
    {
        readonly IOptions<Options.JwtOptions> jwtOptions;

        public TokenTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            jwtOptions = Microsoft.Extensions.Options.Options.Create(new Options.JwtOptions() { Secret = "test123456test123456test123456", Issuer = "issuer", ValidDays = 10 });
        }

        private UserInternalTokenBusiness getUserInternalTokenBusiness()
        {
            return new UserInternalTokenBusiness(DbContext, GetLogger<UserInternalTokenBusiness>(), jwtOptions);
        }

        /// <summary>
        /// This scenario can happen when the user already has an active token but somehow is logged out and tries to log-in.
        /// Therefore a new token is generated for the user, and it is expected that the old token becomes invalid.
        /// </summary>
        [Fact]
        public void WriteToken_Success_DifferentTokenExistsForUser()
        {
            // Arrange
            DbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = "newToken",
                CreatingIp = "1.1.1.1",
                Issuer = jwtOptions.Value.Issuer,
                IsValid = true,
                LastUsedDate = DateTime.UtcNow,
                UserId = "testuser",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(jwtOptions.Value.ValidDays)
            });
            DbContext.SaveChanges();

            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            // Act
            var response = userInternalTokenBusiness.WriteToken("differenttoken", "testuser", "1.1.1.1", DateTime.UtcNow);

            var oldToken = DbContext.UserInternalTokens.Single(q => q.Id == "newToken");

            // Assert
            AssertSuccessCase(response);
            Assert.False(oldToken.IsValid, "Expected original token's validity to be set as false.");
        }

        [Fact]
        public void WriteToken_Success_TokenNotSpecified()
        {
            // Arrange
            DbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = "newToken",
                CreatingIp = "1.1.1.1",
                Issuer = jwtOptions.Value.Issuer,
                IsValid = true,
                LastUsedDate = DateTime.UtcNow,
                UserId = "testuser",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(jwtOptions.Value.ValidDays)
            });
            DbContext.SaveChanges();

            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            // Act
            var response = userInternalTokenBusiness.WriteToken("", "testuser", "1.1.1.1", DateTime.UtcNow);

            var oldToken = DbContext.UserInternalTokens.Single(q => q.Id == "newToken");

            // Assert
            AssertSingleErrorCase(response, ErrorCodes.TOKEN_EMPTY);
            Assert.True(oldToken.IsValid, "Expected original token wouldn't be updated.");
        }

        [Fact]
        public void WriteToken_Success_SameTokenExists()
        {
            // Arrange
            DateTime initialValidTo = DateTime.UtcNow.AddDays(jwtOptions.Value.ValidDays);
            DbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = "newToken",
                CreatingIp = "1.1.1.1",
                Issuer = "issuer",
                IsValid = true,
                LastUsedDate = DateTime.UtcNow,
                UserId = "testuser",
                ValidFrom = DateTime.UtcNow,
                ValidTo = initialValidTo
            });
            DbContext.SaveChanges();

            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            // Act
            Thread.Sleep(1000);
            var response = userInternalTokenBusiness.WriteToken("newToken", "testuser", "1.1.1.1", DateTime.UtcNow);

            var token = DbContext.UserInternalTokens.Single(q => q.Id == "newToken");

            // Assert
            AssertSuccessCase(response);
            Assert.True(token.ValidTo > initialValidTo, "Expected new validTo value to be greater than the initial one.");
        }

        [Fact]
        public void WriteToken_Success_TokenDoesntExists()
        {
            // Arrange
            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            // Act
            userInternalTokenBusiness.WriteToken("newToken", "testuser", "1.1.1.1", DateTime.UtcNow);

            var actual = DbContext.UserInternalTokens.Single(q => q.Id == "newToken");

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void GenerateToken_Success()
        {
            // Arrange
            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            // Act
            string token = userInternalTokenBusiness.GenerateToken("newUser", "1.1.1.1");

            // Assert
            Assert.False(string.IsNullOrEmpty(token), "Expected token string not to be null or empty");
        }

        [Fact]
        public void GetActiveToken_Success()
        {
            // Arrange
            DbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = "newToken",
                CreatingIp = "1.1.1.1",
                Issuer = "issuer",
                IsValid = true,
                LastUsedDate = DateTime.UtcNow,
                UserId = "testuser",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(jwtOptions.Value.ValidDays)
            });
            DbContext.SaveChanges();

            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            // Act
            string token = userInternalTokenBusiness.GetUsersActiveToken("testuser", jwtOptions.Value.Issuer, "1.1.1.1");

            // Assert
            Assert.False(string.IsNullOrEmpty(token), "Expected token string not to be null or empty");
        }

        [Fact]
        public void GetActiveToken_Fail_TokenDoesntExistForUser()
        {
            // Arrange
            DbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = "newToken",
                CreatingIp = "1.1.1.1",
                Issuer = "issuer",
                IsValid = true,
                LastUsedDate = DateTime.UtcNow,
                UserId = "testuser",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(jwtOptions.Value.ValidDays)
            });
            DbContext.SaveChanges();

            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            // Act
            string token = userInternalTokenBusiness.GetUsersActiveToken("newUser", jwtOptions.Value.Issuer, "1.1.1.1");

            // Assert
            Assert.True(string.IsNullOrEmpty(token), "Expected token to be null");
        }

        [Fact]
        public void GetActiveToken_Fail_TokenIsExpired()
        {
            // Arrange
            DbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = "newToken",
                CreatingIp = "1.1.1.1",
                Issuer = "issuer",
                IsValid = true,
                LastUsedDate = DateTime.UtcNow,
                UserId = "testuser",
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(-1)
            });
            DbContext.SaveChanges();

            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            // Act
            string token = userInternalTokenBusiness.GetUsersActiveToken("testuser", jwtOptions.Value.Issuer, "1.1.1.1");

            // Assert
            Assert.True(string.IsNullOrEmpty(token), "Expected token to be null");
        }
    }
}
