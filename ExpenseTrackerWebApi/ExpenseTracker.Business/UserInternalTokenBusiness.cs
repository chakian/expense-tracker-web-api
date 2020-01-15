using System;
using System.Threading.Tasks;
using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Business
{
    public class UserInternalTokenBusiness : BusinessBase<UserInternalTokenBusiness>, IUserInternalTokenBusiness
    {
        private readonly ExpenseTrackerContext dbContext;

        public UserInternalTokenBusiness(ExpenseTrackerContext dbContext, ILogger<UserInternalTokenBusiness> logger)
            : base(logger)
        {
            this.dbContext = dbContext;
        }
        public async Task WriteToken(string tokenString, string userId, string issuer, string creatingIp, DateTime validFrom, DateTime validTo, bool isValid = true)
        {
            //TODO: Learn about refresh tokens!
            var token = await dbContext.UserInternalTokens.SingleOrDefaultAsync(q => q.UserId == userId && q.Issuer == issuer && q.CreatingIp == creatingIp && q.Id == tokenString);
            if (token == null)
            {
                token = await dbContext.UserInternalTokens.SingleOrDefaultAsync(q => q.UserId == userId && q.Issuer == issuer && q.CreatingIp == creatingIp);
                if (token == null)
                {
                    dbContext.UserInternalTokens.Add(new Persistence.Context.DbModels.UserInternalToken()
                    {
                        Id = tokenString,
                        UserId = userId,
                        Issuer = issuer,
                        CreatingIp = creatingIp,
                        ValidFrom = validFrom,
                        ValidTo = validTo,
                        LastUsedDate = DateTime.UtcNow,
                        IsValid = isValid
                    });
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    token.LastUsedDate = DateTime.UtcNow;
                    token.ValidFrom = DateTime.UtcNow.AddDays(60);// TODO: make this a global setting
                    token.Id = tokenString;
                    await dbContext.SaveChangesAsync();
                }
            }
            else
            {
                token.LastUsedDate = DateTime.UtcNow;
                token.ValidFrom = DateTime.UtcNow.AddDays(60);// TODO: make this a global setting
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
