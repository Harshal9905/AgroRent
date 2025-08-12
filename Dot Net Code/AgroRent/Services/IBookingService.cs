using AgroRent.DTOs;

namespace AgroRent.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponseDTO>> GetAllBookingsAsync();
        Task<BookingResponseDTO?> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingResponseDTO>> GetBookingsByFarmerAsync(int farmerId);
        Task<IEnumerable<BookingResponseOwnerDTO>> GetBookingsByEquipmentOwnerAsync(int ownerId);
        Task<BookingResponseDTO> CreateBookingAsync(BookingReqDto dto, int farmerId);
        Task<BookingResponseDTO> UpdateBookingStatusAsync(int id, string status, int userId, bool isOwner);
        Task<bool> CancelBookingAsync(int id, int userId);
        Task<bool> DeleteBookingAsync(int id, int userId);
    }
}
