using AgroRent.DTOs;
using AgroRent.Models;
using AgroRent.Repositories;
using Microsoft.AspNetCore.Http;

namespace AgroRent.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IFarmerRepository _farmerRepository;
        private readonly IImageService _imageService;

        public EquipmentService(
            IEquipmentRepository equipmentRepository,
            IFarmerRepository farmerRepository,
            IImageService imageService)
        {
            _equipmentRepository = equipmentRepository;
            _farmerRepository = farmerRepository;
            _imageService = imageService;
        }

        public async Task<IEnumerable<EquipmentRespDto>> GetAllEquipmentAsync()
        {
            var equipments = await _equipmentRepository.GetAllAsync();
            return equipments.Select(MapToDto);
        }

        public async Task<EquipmentRespDto?> GetEquipmentByIdAsync(int id)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            return equipment != null ? MapToDto(equipment) : null;
        }

        public async Task<IEnumerable<EquipmentRespDto>> GetEquipmentByOwnerAsync(int ownerId)
        {
            var equipments = await _equipmentRepository.GetByOwnerIdAsync(ownerId);
            return equipments.Select(MapToDto);
        }

        public async Task<EquipmentRespDto> AddEquipmentAsync(EquipmentDto dto, int ownerId, IFormFile? image)
        {
            var owner = await _farmerRepository.GetByIdAsync(ownerId);
            if (owner == null)
                throw new InvalidOperationException("Owner not found");

            var equipment = new Equipment
            {
                Name = dto.Name,
                Description = dto.Description,
                RentalPrice = dto.RentalPrice,
                Available = true,
                Owner = owner
            };

            if (image != null)
            {
                equipment.ImageUrl = await _imageService.UploadImageAsync(image);
            }

            var savedEquipment = await _equipmentRepository.AddAsync(equipment);
            return MapToDto(savedEquipment);
        }

        public async Task<EquipmentRespDto> UpdateEquipmentAsync(int id, EquipmentUpdateDto dto, int ownerId)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            if (equipment == null)
                throw new InvalidOperationException("Equipment not found");

            if (equipment.Owner?.Id != ownerId)
                throw new InvalidOperationException("Unauthorized to update this equipment");

            if (dto.Name != null)
                equipment.Name = dto.Name;
            if (dto.Description != null)
                equipment.Description = dto.Description;
            if (dto.RentalPrice.HasValue)
                equipment.RentalPrice = dto.RentalPrice.Value;
            if (dto.Available.HasValue)
                equipment.Available = dto.Available.Value;

            equipment.UpdatedOn = DateTime.Now;

            var updatedEquipment = await _equipmentRepository.UpdateAsync(equipment);
            return MapToDto(updatedEquipment);
        }

        public async Task<bool> DeleteEquipmentAsync(int id, int ownerId)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            if (equipment == null)
                return false;

            if (equipment.Owner?.Id != ownerId)
                return false;

            await _equipmentRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> UpdateAvailabilityAsync(int id, bool available, int ownerId)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            if (equipment == null)
                return false;

            if (equipment.Owner?.Id != ownerId)
                return false;

            equipment.Available = available;
            equipment.UpdatedOn = DateTime.Now;

            await _equipmentRepository.UpdateAsync(equipment);
            return true;
        }

        private static EquipmentRespDto MapToDto(Equipment equipment)
        {
            return new EquipmentRespDto
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Description = equipment.Description,
                ImageUrl = equipment.ImageUrl,
                RentalPrice = equipment.RentalPrice,
                Available = equipment.Available,
                CloudinaryPublicId = equipment.CloudinaryPublicId,
                CreationDate = equipment.CreationDate,
                UpdatedOn = equipment.UpdatedOn,
                OwnerId = equipment.Owner?.Id ?? 0,
                OwnerName = equipment.Owner != null ? $"{equipment.Owner.FirstName} {equipment.Owner.LastName}" : string.Empty
            };
        }
    }
}
