using AgroRent.Data;
using AgroRent.Models;
using Microsoft.EntityFrameworkCore;

namespace AgroRent.Repositories
{
    public class FarmerRepository : IFarmerRepository
    {
        private readonly AgroRentDbContext _context;

        public FarmerRepository(AgroRentDbContext context)
        {
            _context = context;
        }

        public async Task<Farmer?> GetByIdAsync(int id)
        {
            return await _context.Farmers
                .Include(f => f.EquipmentList)
                .Include(f => f.Bookings)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Farmer?> GetByEmailAsync(string email)
        {
            return await _context.Farmers
                .Include(f => f.EquipmentList)
                .Include(f => f.Bookings)
                .FirstOrDefaultAsync(f => f.Email == email);
        }

        public async Task<IEnumerable<Farmer>> GetAllAsync()
        {
            return await _context.Farmers
                .Include(f => f.EquipmentList)
                .Include(f => f.Bookings)
                .ToListAsync();
        }

        public async Task<Farmer> AddAsync(Farmer farmer)
        {
            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();
            return farmer;
        }

        public async Task<Farmer> UpdateAsync(Farmer farmer)
        {
            _context.Farmers.Update(farmer);
            await _context.SaveChangesAsync();
            return farmer;
        }

        public async Task DeleteAsync(int id)
        {
            var farmer = await GetByIdAsync(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Farmers.AnyAsync(f => f.Id == id);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Farmers.AnyAsync(f => f.Email == email);
        }
    }
}
