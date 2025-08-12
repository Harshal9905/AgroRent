using AgroRent.Models;

namespace AgroRent.Repositories
{
    public interface IEquipmentRepository
    {
        Task<Equipment?> GetByIdAsync(int id);
        Task<IEnumerable<Equipment>> GetAllAsync();
        Task<IEnumerable<Equipment>> GetByOwnerIdAsync(int ownerId);
        Task<IEnumerable<Equipment>> GetAvailableAsync();
        Task<Equipment> AddAsync(Equipment equipment);
        Task<Equipment> UpdateAsync(Equipment equipment);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
