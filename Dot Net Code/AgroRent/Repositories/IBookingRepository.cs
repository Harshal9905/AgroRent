using AgroRent.Models;

namespace AgroRent.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetByFarmerIdAsync(int farmerId);
        Task<IEnumerable<Booking>> GetByEquipmentIdAsync(int equipmentId);
        Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status);
        Task<Booking> AddAsync(Booking booking);
        Task<Booking> UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
