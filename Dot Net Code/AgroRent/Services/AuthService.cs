using AgroRent.DTOs;
using AgroRent.Models;
using AgroRent.Repositories;
using AgroRent.Security;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace AgroRent.Services
{
    public class AuthService : IAuthService
    {
        private readonly IFarmerRepository _farmerRepository;
        private readonly IVerificationTokenRepository _verificationTokenRepository;
        private readonly IEmailService _emailService;
        private readonly JwtUtils _jwtUtils;

        public AuthService(
            IFarmerRepository farmerRepository,
            IVerificationTokenRepository verificationTokenRepository,
            IEmailService emailService,
            JwtUtils jwtUtils)
        {
            _farmerRepository = farmerRepository;
            _verificationTokenRepository = verificationTokenRepository;
            _emailService = emailService;
            _jwtUtils = jwtUtils;
        }

        public async Task<AuthResponse> UserSignUpAsync(UserSignUpDto dto)
        {
            if (await _farmerRepository.ExistsByEmailAsync(dto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var hashedPassword = HashPassword(dto.Password);
            var farmer = new Farmer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = hashedPassword,
                Role = Role.ROLE_FARMER,
                Active = false
            };

            var savedFarmer = await _farmerRepository.AddAsync(farmer);

            // Generate verification token
            var token = Guid.NewGuid().ToString();
            var verificationToken = new VerificationToken(token, savedFarmer, DateTime.Now.AddHours(24));
            await _verificationTokenRepository.AddAsync(verificationToken);

            // Send verification email
            await _emailService.SendVerificationEmailAsync(savedFarmer.Email, token);

            return new AuthResponse("User registered successfully. Please check your email for verification.", "");
        }

        public async Task<AuthResponse> UserSignInAsync(UserSignIn dto)
        {
            var farmer = await _farmerRepository.GetByEmailAsync(dto.Email);
            if (farmer == null || !VerifyPassword(dto.Password, farmer.Password))
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            if (!farmer.Active)
            {
                throw new InvalidOperationException("Account not verified. Please check your email for verification.");
            }

            var token = _jwtUtils.GenerateJwtToken(farmer);
            return new AuthResponse("Login successful", token);
        }

        public async Task<ApiResponse<string>> VerifyTokenAsync(string token)
        {
            var verificationToken = await _verificationTokenRepository.GetByTokenAsync(token);
            if (verificationToken == null)
            {
                return ApiResponse<string>.ErrorResponse("Invalid verification token");
            }

            if (verificationToken.ExpiryDate < DateTime.Now)
            {
                return ApiResponse<string>.ErrorResponse("Verification token has expired");
            }

            var farmer = verificationToken.Farmer;
            if (farmer == null)
            {
                return ApiResponse<string>.ErrorResponse("Invalid farmer associated with token");
            }

            farmer.Active = true;
            await _farmerRepository.UpdateAsync(farmer);

            // Delete the used token
            await _verificationTokenRepository.DeleteAsync(verificationToken.Id);

            return ApiResponse<string>.SuccessResponse("Email verified successfully");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
}
