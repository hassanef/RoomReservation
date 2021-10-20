using Identity.Entities.Identity;
using System;
using System.Threading.Tasks;

namespace Identity.Services.Contracts
{
    public interface IUsedPasswordsService
    {
        Task<bool> IsPreviouslyUsedPasswordAsync(User user, string newPassword);
        Task AddToUsedPasswordsListAsync(User user);
        //Task<bool> IsLastUserPasswordTooOldAsync(int userId);
        //Task<DateTime?> GetLastUserPasswordChangeDateAsync(int userId);
    }
}