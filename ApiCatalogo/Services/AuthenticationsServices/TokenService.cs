﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MainBlog.Services.AuthenticationsServices
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                      throw new InvalidOperationException("Invalid secret key");

            var privateKey = Encoding.UTF8.GetBytes(key);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT")
                    .GetValue<double>("TokenValidityInMinutes")),
                Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);
            var refreshToken = Convert.ToBase64String(secureRandomBytes);

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid key");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false // we want to check even if it's expired
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string GeneratePasswordResetToken(string userId, IConfiguration _config)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim("purpose", "password_reset")
        };

            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                      throw new InvalidOperationException("Invalid secret key");
            var privateKey = Encoding.UTF8.GetBytes(key);
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidatePasswordResetToken(string token, IConfiguration _config)
        {
            try
            {
                var secretKey = _config["JWT:SecretKey"] ??
                                throw new InvalidOperationException("Invalid key");

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateLifetime = true
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

                var purposeClaim = principal.Claims.FirstOrDefault(c => c.Type == "purpose");
                return purposeClaim?.Value == "password_reset";
            }
            catch
            {
                return false;
            }
        }
    }
}
