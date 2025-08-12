using AgroRent.Data;
using AgroRent.Models;
using Microsoft.EntityFrameworkCore;

namespace AgroRent.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AgroRentDbContext _context;

        public EquipmentRepository(AgroRentDbContext context)
        {
            _context = context;
        }

        public async Task<Equipment?> GetByIdAsync(int id)
        {
            return await _context.Equipments
                .Include(e => e.Owner)
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Equipment>> GetAllAsync()
        {
            return await _context.Equipments
                .Include(e => e.Owner)
                .Include(e => e.Bookings)
                .ToListAsync();
        }

        public async Task<IEnumerable<Equipment>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Equipments
                .Include(e => e.Owner)
                .Include(e => e.Bookings)
                .Where(e => e.Owner!.Id == ownerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Equipment>> GetAvailableAsync()
        {
            return await _context.Equipments
                .Include(e => e.Owner)
                .Include(e => e.Bookings)
                .Where(e => e.Available)
                .ToListAsync();
        }

        public async Task<Equipment> AddAsync(Equipment equipment)
        {
            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
            return equipment;
        }

        public async Task<Equipment> UpdateAsync(Equipment equipment)
        {
            _context.Equipments.Update(equipment);
            await _context.SaveChangesAsync();
            return equipment;
        }

        public async Task DeleteAsync(int id)
        {
            var equipment = await GetByIdAsync(id);
            if (equipment != null)
            {
                _context.Equipments.Remove(equipment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Equipments.AnyAsync(e => e.Id == id);
        }
    }
}
