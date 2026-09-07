using UserService.Models;

namespace UserService.Repositories
{
    public interface IClientRepository:IRepository<Client>
    {
        Task<bool> GetStateByCI(string CI);
        Task<bool> ExistsByCI(string CI);
        Task<List<Client>> GetPagedAsync(int page, int pageSize);
    }
}
