using ExpenseTracker.Business.Options;
using ExpenseTracker.Models.Base;
using System;

namespace ExpenseTracker.Business.Interfaces
{
    public interface IUserInternalTokenBusiness
    {
        string GenerateToken(string userId, string requestIp, JwtOptions tokenOptions);
        BaseResponse WriteToken(JwtOptions tokenOptions, string token, string userId, string creatingIp, DateTime validFrom, bool isValid = true);
        string GetUsersActiveToken(string userId, string issuer, string creatingIp);
    }
}
