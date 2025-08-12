using AgroRent.DTOs;

namespace AgroRent.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> UserSignUpAsync(UserSignUpDto dto);
        Task<AuthResponse> UserSignInAsync(UserSignIn dto);
        Task<ApiResponse<string>> VerifyTokenAsync(string token);
    }
}
