using AccountService.Data;
using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context) { }

        public async Task<bool> AccountNumberExistsAsync(string accountNumber) =>
            await _dbSet.AnyAsync(a => a.AccountNumber == accountNumber);
    }
}
