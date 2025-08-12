using AgroRent.DTOs;
using AgroRent.Models;
using AgroRent.Repositories;

namespace AgroRent.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IFarmerRepository _farmerRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IEquipmentRepository equipmentRepository,
            IFarmerRepository farmerRepository)
        {
            _bookingRepository = bookingRepository;
            _equipmentRepository = equipmentRepository;
            _farmerRepository = farmerRepository;
        }

        public async Task<IEnumerable<BookingResponseDTO>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Select(MapToDto);
        }

        public async Task<BookingResponseDTO?> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return booking != null ? MapToDto(booking) : null;
        }

        public async Task<IEnumerable<BookingResponseDTO>> GetBookingsByFarmerAsync(int farmerId)
        {
            var bookings = await _bookingRepository.GetByFarmerIdAsync(farmerId);
            return bookings.Select(MapToDto);
        }

        public async Task<IEnumerable<BookingResponseOwnerDTO>> GetBookingsByEquipmentOwnerAsync(int ownerId)
        {
            var equipments = await _equipmentRepository.GetByOwnerIdAsync(ownerId);
            var allBookings = new List<BookingResponseOwnerDTO>();

            foreach (var equipment in equipments)
            {
                var bookings = await _bookingRepository.GetByEquipmentIdAsync(equipment.Id);
                allBookings.AddRange(bookings.Select(b => MapToOwnerDto(b)));
            }

            return allBookings;
        }

        public async Task<BookingResponseDTO> CreateBookingAsync(BookingReqDto dto, int farmerId)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(dto.EquipmentId);
            if (equipment == null)
                throw new InvalidOperationException("Equipment not found");

            if (!equipment.Available)
                throw new InvalidOperationException("Equipment is not available");

            if (equipment.Owner?.Id == farmerId)
                throw new InvalidOperationException("Cannot book your own equipment");

            var farmer = await _farmerRepository.GetByIdAsync(farmerId);
            if (farmer == null)
                throw new InvalidOperationException("Farmer not found");

            var days = (dto.EndDate - dto.StartDate).Days + 1;
            var totalAmount = days * equipment.RentalPrice;

            var booking = new Booking
            {
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = BookingStatus.PENDING,
                TotalAmount = totalAmount,
                Equipment = equipment,
                Farmer = farmer
            };

            var savedBooking = await _bookingRepository.AddAsync(booking);
            return MapToDto(savedBooking);
        }

        public async Task<BookingResponseDTO> UpdateBookingStatusAsync(int id, string status, int userId, bool isOwner)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            if (isOwner && booking.Equipment?.Owner?.Id != userId)
                throw new InvalidOperationException("Unauthorized to update this booking");

            if (!isOwner && booking.Farmer?.Id != userId)
                throw new InvalidOperationException("Unauthorized to update this booking");

            if (Enum.TryParse<BookingStatus>(status, true, out var bookingStatus))
            {
                booking.Status = bookingStatus;
                booking.UpdatedOn = DateTime.Now;

                var updatedBooking = await _bookingRepository.UpdateAsync(booking);
                return MapToDto(updatedBooking);
            }

            throw new InvalidOperationException("Invalid booking status");
        }

        public async Task<bool> CancelBookingAsync(int id, int userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                return false;

            if (booking.Farmer?.Id != userId && booking.Equipment?.Owner?.Id != userId)
                return false;

            if (booking.Status != BookingStatus.PENDING && booking.Status != BookingStatus.ACCEPTED)
                return false;

            booking.Status = BookingStatus.CANCELLED;
            booking.UpdatedOn = DateTime.Now;

            await _bookingRepository.UpdateAsync(booking);
            return true;
        }

        public async Task<bool> DeleteBookingAsync(int id, int userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                return false;

            if (booking.Farmer?.Id != userId && booking.Equipment?.Owner?.Id != userId)
                return false;

            await _bookingRepository.DeleteAsync(id);
            return true;
        }

        private static BookingResponseDTO MapToDto(Booking booking)
        {
            return new BookingResponseDTO
            {
                Id = booking.Id,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status.ToString(),
                TotalAmount = booking.TotalAmount,
                CreationDate = booking.CreationDate,
                UpdatedOn = booking.UpdatedOn,
                EquipmentId = booking.Equipment?.Id ?? 0,
                EquipmentName = booking.Equipment?.Name ?? string.Empty,
                FarmerId = booking.Farmer?.Id ?? 0,
                FarmerName = booking.Farmer != null ? $"{booking.Farmer.FirstName} {booking.Farmer.LastName}" : string.Empty
            };
        }

        private static BookingResponseOwnerDTO MapToOwnerDto(Booking booking)
        {
            return new BookingResponseOwnerDTO
            {
                Id = booking.Id,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Status = booking.Status.ToString(),
                TotalAmount = booking.TotalAmount,
                CreationDate = booking.CreationDate,
                UpdatedOn = booking.UpdatedOn,
                EquipmentId = booking.Equipment?.Id ?? 0,
                EquipmentName = booking.Equipment?.Name ?? string.Empty,
                RenterId = booking.Farmer?.Id ?? 0,
                RenterName = booking.Farmer != null ? $"{booking.Farmer.FirstName} {booking.Farmer.LastName}" : string.Empty,
                RenterEmail = booking.Farmer?.Email ?? string.Empty
            };
        }
    }
}
