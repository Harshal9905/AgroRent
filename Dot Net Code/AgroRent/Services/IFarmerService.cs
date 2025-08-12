using AgroRent.DTOs;

namespace AgroRent.Services
{
    public interface IFarmerService
    {
        Task<IEnumerable<FarmerResponseDto>> GetAllFarmersAsync();
        Task<FarmerResponseDto?> GetFarmerByIdAsync(int id);
        Task<FarmerResponseDto> UpdateFarmerAsync(int id, FarmerResponseDto dto);
        Task<bool> DeleteFarmerAsync(int id);
        Task<bool> DeactivateFarmerAsync(int id);
        Task<bool> ActivateFarmerAsync(int id);
    }
}
