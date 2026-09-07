namespace AccountService.Repositories
{
    public interface IUnitofWork
    {
        IAccountRepository Accounts { get; }
        IMovementRepository Movements { get; }

        Task<int> SaveChangesAsync();
    }
}
