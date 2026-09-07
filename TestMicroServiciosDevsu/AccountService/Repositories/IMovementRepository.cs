using AccountService.Models;

namespace AccountService.Repositories
{
    public interface IMovementRepository : IRepository<Movement>
    {
        // Historial de una cuenta puntual, con filtro opcional de fechas
        Task<List<Movement>> GetByAccountIdAsync(Guid accountId);

        Task<List<Movement>> GetFilteredAsync(
             Guid? clientId,
             DateTime? desde,
             DateTime? hasta);


    }
}
