using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MainBlog.Services.AuthenticationsServices
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
        string GeneratePasswordResetToken(string userId, IConfiguration _config);
        bool ValidatePasswordResetToken(string token, IConfiguration _config);
    }
}
