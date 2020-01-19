using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Models.UserModels;
using Microsoft.Extensions.Options;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.UserTests
{
    public class TokenTests : UnitTestBase
    {
        readonly IOptions<Options.JwtOptions> jwtOptions;

        public TokenTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            jwtOptions = Microsoft.Extensions.Options.Options.Create(new Options.JwtOptions() { Secret = "test123456test123456test123456" });
        }

        private UserInternalTokenBusiness getUserInternalTokenBusiness()
        {
            return new UserInternalTokenBusiness(DbContext, GetLogger<UserInternalTokenBusiness>(), jwtOptions);
        }

        [Fact]
        public void WriteToken_Success_TokenExistsForUser()
        {
            // Arrange
            DbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = "newToken",
                CreatingIp = "1.1.1.1",
                Issuer = "issuer",
                IsValid = true,
                LastUsedDate = DateTime.Now,
                UserId = "testuser",
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddDays(60)
            });
            DbContext.SaveChanges();

            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();
            
            AuthenticateUserRequest authenticateUserRequest = new AuthenticateUserRequest()
            {
                Email = "test@test.com",
                Password = "123456",
                Culture = "",
                RequestIp = "1.1.1.1"
            };

            // Act
            userInternalTokenBusiness.WriteToken("differenttoken", "testuser", "issuer", "1.1.1.1", DateTime.Now, DateTime.Now.AddDays(60));
        }

        [Fact]
        public void WriteToken_Success_SameTokenExists()
        {
            // Arrange
            DbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
            {
                Id = "newToken",
                CreatingIp = "1.1.1.1",
                Issuer = "issuer",
                IsValid = true,
                LastUsedDate = DateTime.Now,
                UserId = "testuser",
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddDays(60)
            });
            DbContext.SaveChanges();

            IUserInternalTokenBusiness userInternalTokenBusiness = getUserInternalTokenBusiness();

            AuthenticateUserRequest authenticateUserRequest = new AuthenticateUserRequest()
            {
                Email = "test@test.com",
                Password = "123456",
                Culture = "",
                RequestIp = "1.1.1.1"
            };

            // Act
            userInternalTokenBusiness.WriteToken("newToken", "testuser", "issuer", "1.1.1.1", DateTime.Now, DateTime.Now.AddDays(60));
        }
    }
}
