using UserService.Data;
using UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context) : base(context) { }

        public async Task<bool> GetStateByCI(string CI) =>
            await _dbSet
            .Where(c=>c.CI==CI)
            .Select(c=>c.State)
            .FirstOrDefaultAsync();

        public async Task<bool> ExistsByCI(string CI) =>
        await _dbSet.AnyAsync(c => c.CI == CI);

        public async Task<List<Client>> GetPagedAsync(int page, int pageSize) =>
            await _dbSet
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}
