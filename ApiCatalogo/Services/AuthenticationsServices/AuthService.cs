using MainBlog.Controller;
using MainBlog.DTOs.AuthenticationsDTO;
using MainBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MainBlog.Services.AuthenticationsServices
{
    public class AuthService : IAuthService
    {
        private ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthService(){}

        public AuthService(ITokenService tokenService,
                              RoleManager<IdentityRole> roleManager,
                              UserManager<ApplicationUser> userManager,
                              IConfiguration config,
                              ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }
        public async Task<ResponseModel> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            roleName = roleName.ToUpper();
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                    return new ResponseModel { Status = "Success", Message = $"User {user.Email} added to the {roleName} role" };
                }
                else
                {
                    return new ResponseModel { Status = "Error", Message = $"Error: Unable to add user {user.Email} to the {roleName} role" };
                }
            }
            return new ResponseModel { Status = "Error", Message = $"Unable to find user" };
        }

        public async Task<ResponseModel> CreateRole(string roleName)
        {
            roleName = roleName.ToUpper();
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return new ResponseModel { Status = "Success", Message = $"Role {roleName} added successfully" };
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return new ResponseModel { Status = "Error", Message = $"Issue adding the new {roleName} role" };
                }
            }
            return new ResponseModel { Status = "Error", Message = "Role already exist." };
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
    }
}
