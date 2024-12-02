using MainBlog.DTOs.AuthenticationsDTO;
using MainBlog.Models.ResultModels;
using Microsoft.AspNetCore.Identity;

namespace MainBlog.Services.AuthenticationsServices
{
    public interface IAuthService
    {
        Task<List<UserWithRolesDto>> FindAllUsersWithRoles();
        Task<IEnumerable<IdentityRole>> GetRoles();
        Task<object> LoginAsync(LoginModel model);
        Task<object> Register(RegisterModel model);
        Task<object> RefreshToken(TokenModel tokenModel);
        Task<bool> RevokeToken(string username);
        Task<Result> CreateRole(string roleName);
        Task<Result> AddUserToRole(string email, string roleName);
        Task<Result> ForgotPassword(string email);
        Task<Result> ResetPassword(string token, string newPassword);
    }
}
