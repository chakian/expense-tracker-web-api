using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Interfaces
{
    public interface IUserInternalTokenBusiness
    {
        Task WriteToken(string token, string userId, string issuer, string creatingIp, DateTime validFrom, DateTime validTo, bool isValid = true);
    }
}
