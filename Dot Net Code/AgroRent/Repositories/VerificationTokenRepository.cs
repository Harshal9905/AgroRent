using AgroRent.Data;
using AgroRent.Models;
using Microsoft.EntityFrameworkCore;

namespace AgroRent.Repositories
{
    public class VerificationTokenRepository : IVerificationTokenRepository
    {
        private readonly AgroRentDbContext _context;

        public VerificationTokenRepository(AgroRentDbContext context)
        {
            _context = context;
        }

        public async Task<VerificationToken?> GetByTokenAsync(string token)
        {
            return await _context.VerificationTokens
                .Include(vt => vt.Farmer)
                .FirstOrDefaultAsync(vt => vt.Token == token);
        }

        public async Task<VerificationToken> AddAsync(VerificationToken token)
        {
            _context.VerificationTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task DeleteAsync(int id)
        {
            var token = await _context.VerificationTokens.FindAsync(id);
            if (token != null)
            {
                _context.VerificationTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteExpiredTokensAsync()
        {
            var expiredTokens = await _context.VerificationTokens
                .Where(vt => vt.ExpiryDate < DateTime.Now)
                .ToListAsync();

            _context.VerificationTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }
    }
}
