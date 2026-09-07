using AccountService.Data;
using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Repositories
{
    public class MovementRepository : Repository<Movement>, IMovementRepository
    {
        public MovementRepository(AppDbContext context) : base(context) { }

            public async Task<List<Movement>> GetByAccountIdAsync(Guid accountId)
    {
             
            return await _dbSet.Where(c => c.AccountId == accountId).ToListAsync();
    }
 


    }
}
