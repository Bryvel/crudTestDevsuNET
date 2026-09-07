using AccountService.Models;

namespace AccountService.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<bool> AccountNumberExistsAsync(string accountNumber);
    }
    
    }

