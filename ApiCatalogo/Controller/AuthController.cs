using MainBlog.DTOs.AuthenticationsDTO;
using MainBlog.Models;
using MainBlog.Services.AuthenticationsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MainBlog.IRepository;
using Microsoft.AspNetCore.Identity.Data;
using MainBlog.DTOs.Request;

namespace MainBlog.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> findAllUsersWithRoles()
    {
        return Ok(await _authService.FindAllUsersWithRoles()); 
    }

    [HttpGet]
    [Route("GetRoles")]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _authService.GetRoles());
    }


    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var result = await _authService.LoginAsync(model);

        if (result is ResponseModel)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost] 
    [Route("Register")] 
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        return Ok(await _authService.Register(model));
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        return new ObjectResult(await _authService.RefreshToken(tokenModel));
    }

    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        return Ok(await _authService.RevokeToken(username));
    }

    [HttpPost]
    [Route("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var result = await _authService.CreateRole(roleName);
        if(!result.IsSuccess)
            return BadRequest(result.Error);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    [Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var result = await _authService.AddUserToRole(email, roleName);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { Message = $"User {email} added to role {roleName}" });
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] RequestEmailDTO email)
    {
        var result = await _authService.ForgotPassword(email.Email);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { Message = "Password reset link has been sent to your email." });
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
    {
        var result = await _authService.ResetPassword(request.Token, request.NewPassword);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { Message = "Password has been reset successfully" });
    }
}
