using AgroRent.Data;
using AgroRent.Models;
using Microsoft.EntityFrameworkCore;

namespace AgroRent.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AgroRentDbContext _context;

        public BookingRepository(AgroRentDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Equipment)
                .Include(b => b.Farmer)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Equipment)
                .Include(b => b.Farmer)
                .Include(b => b.Payment)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByFarmerIdAsync(int farmerId)
        {
            return await _context.Bookings
                .Include(b => b.Equipment)
                .Include(b => b.Farmer)
                .Include(b => b.Payment)
                .Where(b => b.Farmer!.Id == farmerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByEquipmentIdAsync(int equipmentId)
        {
            return await _context.Bookings
                .Include(b => b.Equipment)
                .Include(b => b.Farmer)
                .Include(b => b.Payment)
                .Where(b => b.Equipment!.Id == equipmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status)
        {
            return await _context.Bookings
                .Include(b => b.Equipment)
                .Include(b => b.Farmer)
                .Include(b => b.Payment)
                .Where(b => b.Status == status)
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await GetByIdAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Bookings.AnyAsync(b => b.Id == id);
        }
    }
}
