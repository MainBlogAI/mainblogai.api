using Humanizer;
using MainBlog.Controller;
using MainBlog.DTOs.AuthenticationsDTO;
using MainBlog.Models;
using MainBlog.Models.ResultModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Session;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;

namespace MainBlog.Services.AuthenticationsServices
{
    public class AuthService : IAuthService
    {
        private ITokenService _tokenService;
        private IEmailSenderService _emailSenderService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            IHttpContextAccessor httpContextAccessor, 
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration config,
            ILogger<AuthController> logger,
            IEmailSenderService emailSenderService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            _emailSenderService = emailSenderService;
        }

        public async Task<List<UserWithRolesDto>> FindAllUsersWithRoles()
        {
            var usersWithRoles = new List<UserWithRolesDto>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user) ?? new List<string>();
                var userWithRoles = new UserWithRolesDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = userRoles
                };
                usersWithRoles.Add(userWithRoles);
            }

            return usersWithRoles;
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Object> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username!);
            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = _tokenService.GenerateAccessToken(authClaims, _config);
                var refreshToken = _tokenService.GenerateRefreshToken();
                _ = int.TryParse(_config["JWT:RefreshTokenValidityInMinutes"],
                                      out int refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime =
                    DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);
                await _userManager.UpdateAsync(user);

                return new 
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    RefreshToken = refreshToken
                };
            }
            return new ResponseModel { Status = "Error", Message = "Login ou senha inválida." };
        }

        public async Task<object> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return ("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken
                ?? throw new ArgumentNullException(nameof(tokenModel));
            string? refreshToken = tokenModel.RefreshToken
                ?? throw new ArgumentNullException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _config);

            if (principal == null)
            {
                return ("Invalid access token/refresh token");
            }
            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username!);
            if (user == null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return ("Invalid access token/refresh token");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(
                principal.Claims.ToList(), _config);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            return new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            };
        }

        public async Task<Object> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username!);

            var emailExists = await _userManager.FindByEmailAsync(model.Email!);
            if (userExists is not null || emailExists is not null)
                return new ResponseModel { Status = "Error", Message = "User or Email already exists!" };

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password!);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                new ResponseModel { Status = "Error", Message = $"User creation failed! Errors: {string.Join(", ", errors)}" };
            }

            return await this.LoginAsync(new LoginModel(model.Username, model.Password));
        }

        public async Task<bool> RevokeToken(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null) return false;
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<Result> CreateRole(string roleName)
        {
            roleName = roleName.ToUpper();
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return Result.Success();
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return Result.Failure(Error.Failure("RoleCreationError", $"Issue adding the new {roleName} role"));
                }
            }
            return Result.Failure(Error.Conflict("RoleExists", $"Role {roleName} already exists."));
        }


        public async Task<Result> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure(Error.NotFound("UserNotFound", "Email not found"));

            var token = _tokenService.GeneratePasswordResetToken(user.Id, _config);

            var request = _httpContextAccessor?.HttpContext?.Request;
            var resetLink = $"http://localhost:4200/resetPassword?token={Uri.EscapeDataString(token)}";

            await _emailSenderService.SendEmailAsync(user.Email, "Password Reset Request",
                $"Click the link to reset your password: <a href='{resetLink}'>Reset Password</a>");

            return Result.Success();
        }

        public async Task<Result> ResetPassword(string token, string newPassword)
        {
            if (!_tokenService.ValidatePasswordResetToken(token, _config))
                return Result.Failure(Error.Validation("InvalidToken", "Invalid or expired token"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userId = jwtToken.Claims.First(claim => claim.Type == "sub").Value;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(Error.NotFound("UserNotFound", "User not found"));

            var resetPassResult = await _userManager.ResetPasswordAsync(
                user,
                await _userManager.GeneratePasswordResetTokenAsync(user),
                newPassword
            );

            return resetPassResult.Succeeded
                ? Result.Success()
                : Result.Failure(Error.Validation("PasswordResetError", string.Join("; ", resetPassResult.Errors.Select(e => e.Description))));
        }

        public async Task<Result> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            roleName = roleName.ToUpper();

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                    return Result.Success();
                }
                else
                {
                    return Result.Failure(Error.Failure("RoleAssignmentError", $"Unable to add user {user.Email} to the {roleName} role"));
                }
            }

            return Result.Failure(Error.NotFound("UserNotFound", "Unable to find user"));
        }
    }
}
