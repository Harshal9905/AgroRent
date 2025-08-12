using AgroRent.DTOs;
using AgroRent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroRent.Controllers
{
    [ApiController]
    [Route("payment")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                return Ok(ApiResponse<IEnumerable<PaymentRespDto>>.SuccessResponse("Payments retrieved successfully", payments));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<IEnumerable<PaymentRespDto>>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                    return NotFound(ApiResponse<PaymentRespDto>.ErrorResponse("Payment not found"));

                return Ok(ApiResponse<PaymentRespDto>.SuccessResponse("Payment retrieved successfully", payment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PaymentRespDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetPaymentByBookingId(int bookingId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
                if (payment == null)
                    return NotFound(ApiResponse<PaymentRespDto>.ErrorResponse("Payment not found"));

                return Ok(ApiResponse<PaymentRespDto>.SuccessResponse("Payment retrieved successfully", payment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PaymentRespDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromQuery] int bookingId, [FromQuery] double amount)
        {
            try
            {
                var payment = await _paymentService.CreatePaymentAsync(bookingId, amount);
                return Created(string.Empty, ApiResponse<PaymentRespDto>.SuccessResponse("Payment created successfully", payment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PaymentRespDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentVerificationRequestDto dto)
        {
            try
            {
                var payment = await _paymentService.VerifyPaymentAsync(dto);
                return Ok(ApiResponse<PaymentRespDto>.SuccessResponse("Payment verified successfully", payment));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PaymentRespDto>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] string status)
        {
            try
            {
                var result = await _paymentService.UpdatePaymentStatusAsync(id, status);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Payment not found"));

                return Ok(ApiResponse<string>.SuccessResponse("Payment status updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var result = await _paymentService.DeletePaymentAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.ErrorResponse("Payment not found"));

                return Ok(ApiResponse<string>.SuccessResponse("Payment deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
