using AgroRent.DTOs;
using AgroRent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgroRent.Controllers
{
    [ApiController]
    [Route("booking")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                return Ok(ApiResponse<IEnumerable<BookingResponseDTO>>.SuccessResponse("Bookings retrieved successfully", bookings));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<BookingResponseDTO>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null)
                    return NotFound(ApiResponse<BookingResponseDTO>.ErrorResponse("Booking not found"));

                return Ok(ApiResponse<BookingResponseDTO>.SuccessResponse("Booking retrieved successfully", booking));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BookingResponseDTO>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("farmer/{farmerId}")]
        public async Task<IActionResult> GetBookingsByFarmer(int farmerId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByFarmerAsync(farmerId);
                return Ok(ApiResponse<IEnumerable<BookingResponseDTO>>.SuccessResponse("Bookings retrieved successfully", bookings));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<BookingResponseDTO>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetBookingsByEquipmentOwner(int ownerId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByEquipmentOwnerAsync(ownerId);
                return Ok(ApiResponse<IEnumerable<BookingResponseOwnerDTO>>.SuccessResponse("Bookings retrieved successfully", bookings));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<BookingResponseOwnerDTO>>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingReqDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var booking = await _bookingService.CreateBookingAsync(dto, userId);
                return Created(string.Empty, ApiResponse<BookingResponseDTO>.SuccessResponse("Booking created successfully", booking));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BookingResponseDTO>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] string status)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var isOwner = User.FindFirst(ClaimTypes.Role)?.Value == "ROLE_ADMIN";
                var booking = await _bookingService.UpdateBookingStatusAsync(id, status, userId, isOwner);
                return Ok(ApiResponse<BookingResponseDTO>.SuccessResponse("Booking status updated successfully", booking));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<BookingResponseDTO>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _bookingService.CancelBookingAsync(id, userId);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Booking not found or cannot be cancelled"));

                return Ok(ApiResponse<string>.SuccessResponse("Booking cancelled successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _bookingService.DeleteBookingAsync(id, userId);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Booking not found or unauthorized"));

                return Ok(ApiResponse<string>.SuccessResponse("Booking deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
