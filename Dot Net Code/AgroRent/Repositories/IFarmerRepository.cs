using AgroRent.Models;

namespace AgroRent.Repositories
{
    public interface IFarmerRepository
    {
        Task<Farmer?> GetByIdAsync(int id);
        Task<Farmer?> GetByEmailAsync(string email);
        Task<IEnumerable<Farmer>> GetAllAsync();
        Task<Farmer> AddAsync(Farmer farmer);
        Task<Farmer> UpdateAsync(Farmer farmer);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
