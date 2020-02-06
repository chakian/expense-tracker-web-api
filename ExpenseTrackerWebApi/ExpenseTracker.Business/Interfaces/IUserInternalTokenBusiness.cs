using ExpenseTracker.Models.Base;
using System;

namespace ExpenseTracker.Business.Interfaces
{
    public interface IUserInternalTokenBusiness
    {
        string GenerateToken(string userId, string requestIp);
        BaseResponse WriteToken(string token, string userId, string creatingIp, string device, DateTime validFrom, bool isValid = true);
        string GetUsersActiveToken(string userId, string issuer, string creatingIp, string device);
    }
}
