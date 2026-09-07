using AccountService.Data;

namespace AccountService.Repositories;

public class UnitofWork : IUnitofWork
{
    private readonly AppDbContext _context;

    public IAccountRepository Accounts { get; }
    public IMovementRepository Movements { get; }

    public UnitofWork(AppDbContext context)
    {
        _context = context;
        Accounts = new AccountRepository(context);
        Movements = new MovementRepository(context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
