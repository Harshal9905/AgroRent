using AgroRent.DTOs;
using AgroRent.Models;
using AgroRent.Repositories;

namespace AgroRent.Services
{
    public class FarmerService : IFarmerService
    {
        private readonly IFarmerRepository _farmerRepository;

        public FarmerService(IFarmerRepository farmerRepository)
        {
            _farmerRepository = farmerRepository;
        }

        public async Task<IEnumerable<FarmerResponseDto>> GetAllFarmersAsync()
        {
            var farmers = await _farmerRepository.GetAllAsync();
            return farmers.Select(MapToDto);
        }

        public async Task<FarmerResponseDto?> GetFarmerByIdAsync(int id)
        {
            var farmer = await _farmerRepository.GetByIdAsync(id);
            return farmer != null ? MapToDto(farmer) : null;
        }

        public async Task<FarmerResponseDto> UpdateFarmerAsync(int id, FarmerResponseDto dto)
        {
            var farmer = await _farmerRepository.GetByIdAsync(id);
            if (farmer == null)
                throw new InvalidOperationException("Farmer not found");

            farmer.FirstName = dto.FirstName;
            farmer.LastName = dto.LastName;
            farmer.Email = dto.Email;
            farmer.Role = Enum.Parse<Role>(dto.Role);
            farmer.Active = dto.Active;
            farmer.UpdatedOn = DateTime.Now;

            var updatedFarmer = await _farmerRepository.UpdateAsync(farmer);
            return MapToDto(updatedFarmer);
        }

        public async Task<bool> DeleteFarmerAsync(int id)
        {
            var farmer = await _farmerRepository.GetByIdAsync(id);
            if (farmer == null)
                return false;

            await _farmerRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> DeactivateFarmerAsync(int id)
        {
            var farmer = await _farmerRepository.GetByIdAsync(id);
            if (farmer == null)
                return false;

            farmer.Active = false;
            farmer.UpdatedOn = DateTime.Now;

            await _farmerRepository.UpdateAsync(farmer);
            return true;
        }

        public async Task<bool> ActivateFarmerAsync(int id)
        {
            var farmer = await _farmerRepository.GetByIdAsync(id);
            if (farmer == null)
                return false;

            farmer.Active = true;
            farmer.UpdatedOn = DateTime.Now;

            await _farmerRepository.UpdateAsync(farmer);
            return true;
        }

        private static FarmerResponseDto MapToDto(Farmer farmer)
        {
            return new FarmerResponseDto
            {
                Id = farmer.Id,
                FirstName = farmer.FirstName,
                LastName = farmer.LastName,
                Email = farmer.Email,
                Role = farmer.Role.ToString(),
                Active = farmer.Active,
                CreationDate = farmer.CreationDate,
                UpdatedOn = farmer.UpdatedOn
            };
        }
    }
}
