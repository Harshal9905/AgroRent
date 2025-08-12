using AgroRent.DTOs;
using AgroRent.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgroRent.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> UserSignUp([FromBody] UserSignUpDto dto)
        {
            try
            {
                var response = await _authService.UserSignUpAsync(dto);
                return Created(string.Empty, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> UserSignIn([FromBody] UserSignIn dto)
        {
            try
            {
                var response = await _authService.UserSignInAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyUserEmail([FromQuery] string token)
        {
            try
            {
                var response = await _authService.VerifyTokenAsync(token);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(false, ex.Message));
            }
        }
    }
}
