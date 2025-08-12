using AgroRent.Models;

namespace AgroRent.Repositories
{
    public interface IVerificationTokenRepository
    {
        Task<VerificationToken?> GetByTokenAsync(string token);
        Task<VerificationToken> AddAsync(VerificationToken token);
        Task DeleteAsync(int id);
        Task DeleteExpiredTokensAsync();
    }
}
