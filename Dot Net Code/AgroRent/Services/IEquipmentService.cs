using AgroRent.DTOs;
using Microsoft.AspNetCore.Http;

namespace AgroRent.Services
{
    public interface IEquipmentService
    {
        Task<IEnumerable<EquipmentRespDto>> GetAllEquipmentAsync();
        Task<EquipmentRespDto?> GetEquipmentByIdAsync(int id);
        Task<IEnumerable<EquipmentRespDto>> GetEquipmentByOwnerAsync(int ownerId);
        Task<EquipmentRespDto> AddEquipmentAsync(EquipmentDto dto, int ownerId, IFormFile? image);
        Task<EquipmentRespDto> UpdateEquipmentAsync(int id, EquipmentUpdateDto dto, int ownerId);
        Task<bool> DeleteEquipmentAsync(int id, int ownerId);
        Task<bool> UpdateAvailabilityAsync(int id, bool available, int ownerId);
    }
}
