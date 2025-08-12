using AgroRent.DTOs;
using AgroRent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroRent.Controllers
{
    [ApiController]
    [Route("farmer")]
    [Authorize]
    public class FarmerController : ControllerBase
    {
        private readonly IFarmerService _farmerService;

        public FarmerController(IFarmerService farmerService)
        {
            _farmerService = farmerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFarmers()
        {
            try
            {
                var farmers = await _farmerService.GetAllFarmersAsync();
                return Ok(ApiResponse<IEnumerable<FarmerResponseDto>>.SuccessResponse("Farmers retrieved successfully", farmers));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<FarmerResponseDto>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFarmerById(int id)
        {
            try
            {
                var farmer = await _farmerService.GetFarmerByIdAsync(id);
                if (farmer == null)
                    return NotFound(ApiResponse<FarmerResponseDto>.ErrorResponse("Farmer not found"));

                return Ok(ApiResponse<FarmerResponseDto>.SuccessResponse("Farmer retrieved successfully", farmer));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<FarmerResponseDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFarmer(int id, [FromBody] FarmerResponseDto dto)
        {
            try
            {
                var farmer = await _farmerService.UpdateFarmerAsync(id, dto);
                return Ok(ApiResponse<FarmerResponseDto>.SuccessResponse("Farmer updated successfully", farmer));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<FarmerResponseDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFarmer(int id)
        {
            try
            {
                var result = await _farmerService.DeleteFarmerAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Farmer not found"));

                return Ok(ApiResponse<string>.SuccessResponse("Farmer deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateFarmer(int id)
        {
            try
            {
                var result = await _farmerService.DeactivateFarmerAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Farmer not found"));

                return Ok(ApiResponse<string>.SuccessResponse("Farmer deactivated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateFarmer(int id)
        {
            try
            {
                var result = await _farmerService.ActivateFarmerAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Farmer not found"));

                return Ok(ApiResponse<string>.SuccessResponse("Farmer activated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
