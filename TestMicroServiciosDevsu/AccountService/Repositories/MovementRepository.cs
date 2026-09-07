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

        public async Task<List<Movement>> GetFilteredAsync(
            Guid? clientId,
            DateTime? desde,
            DateTime? hasta)
        {
            var query = _dbSet.Include(m => m.Account).AsQueryable();

            if (clientId is not null)
                query = query.Where(m => m.Account!.ClientId == clientId);

            if (desde is not null)
                query = query.Where(m => m.Date >= desde);

            if (hasta is not null)
                query = query.Where(m => m.Date <= hasta);

            return await query.OrderByDescending(m => m.Date).ToListAsync();
        }



    }
}
