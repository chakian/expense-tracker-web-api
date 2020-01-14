using System;
using System.Threading.Tasks;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Business
{
    public class UserInternalTokenBusiness : IUserInternalTokenBusiness
    {
        private readonly ExpenseTrackerContext dbContext;

        public UserInternalTokenBusiness(ExpenseTrackerContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async void WriteToken(string tokenString, string userId, string issuer, string creatingIp, DateTime validFrom, DateTime validTo, bool isValid = true)
        {
            //ExpenseTrackerContext context = dbContext;
            _ = Task.Delay(10000);
            var token = await dbContext.UserInternalTokens.SingleOrDefaultAsync(q => q.UserId == userId && q.Id == tokenString && q.Issuer == issuer);
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
                _ = dbContext.SaveChangesAsync();
            }
            else
            {
                token.LastUsedDate = DateTime.UtcNow;
                token.ValidFrom = DateTime.UtcNow.AddDays(60);// TODO: make this a global setting
                _ = dbContext.SaveChangesAsync();
            }
        }
    }
}
