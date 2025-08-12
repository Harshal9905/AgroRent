using AgroRent.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AgroRent.Security
{
    public class JwtCustomFilter : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly JwtUtils _jwtUtils;

        public JwtCustomFilter(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            JwtUtils jwtUtils)
            : base(options, logger, encoder, clock)
        {
            _jwtUtils = jwtUtils;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header not found.");
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Bearer token not found.");
            }

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Token is empty.");
            }

            var principal = _jwtUtils.ValidateToken(token);
            if (principal == null)
            {
                return AuthenticateResult.Fail("Invalid token.");
            }

            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
