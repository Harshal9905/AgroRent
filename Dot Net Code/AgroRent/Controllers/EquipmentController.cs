using AgroRent.DTOs;
using AgroRent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgroRent.Controllers
{
    [ApiController]
    [Route("equipment")]
    [Authorize]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEquipment()
        {
            try
            {
                var equipment = await _equipmentService.GetAllEquipmentAsync();
                return Ok(ApiResponse<IEnumerable<EquipmentRespDto>>.SuccessResponse("Equipment retrieved successfully", equipment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<EquipmentRespDto>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipmentById(int id)
        {
            try
            {
                var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
                if (equipment == null)
                    return NotFound(ApiResponse<EquipmentRespDto>.ErrorResponse("Equipment not found"));

                return Ok(ApiResponse<EquipmentRespDto>.SuccessResponse("Equipment retrieved successfully", equipment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EquipmentRespDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetEquipmentByOwner(int ownerId)
        {
            try
            {
                var equipment = await _equipmentService.GetEquipmentByOwnerAsync(ownerId);
                return Ok(ApiResponse<IEnumerable<EquipmentRespDto>>.SuccessResponse("Equipment retrieved successfully", equipment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<EquipmentRespDto>>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEquipment([FromForm] EquipmentDto dto, [FromForm] IFormFile? image)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var equipment = await _equipmentService.AddEquipmentAsync(dto, userId, image);
                return Created(string.Empty, ApiResponse<EquipmentRespDto>.SuccessResponse("Equipment added successfully", equipment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EquipmentRespDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, [FromBody] EquipmentUpdateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var equipment = await _equipmentService.UpdateEquipmentAsync(id, dto, userId);
                return Ok(ApiResponse<EquipmentRespDto>.SuccessResponse("Equipment updated successfully", equipment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<EquipmentRespDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _equipmentService.DeleteEquipmentAsync(id, userId);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Equipment not found or unauthorized"));

                return Ok(ApiResponse<string>.SuccessResponse("Equipment deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}/availability")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] bool available)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _equipmentService.UpdateAvailabilityAsync(id, available, userId);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Equipment not found or unauthorized"));

                return Ok(ApiResponse<string>.SuccessResponse("Availability updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
