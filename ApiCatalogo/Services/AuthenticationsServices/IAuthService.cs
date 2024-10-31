using MainBlog.DTOs.AuthenticationsDTO;
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
        Task<ResponseModel> CreateRole(string roleName);
        Task<ResponseModel> AddUserToRole(string email, string roleName);
    }
}
