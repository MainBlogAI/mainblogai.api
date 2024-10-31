using MainBlog.DTOs.AuthenticationsDTO;
using MainBlog.Models;
using MainBlog.Services.AuthenticationsServices;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using MainBlog.IRepository;

namespace MainBlog.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    // token service é para gerar tokens
    private ITokenService _tokenService;
    // role manager é para criar e gerenciar roles
    private readonly RoleManager<IdentityRole> _roleManager;
    // user manager é para criar e gerenciar usuários
    private readonly UserManager<ApplicationUser> _userManager;
    // IConfiguration é para acessar o appsettings.json
    private readonly IConfiguration _config;

    private readonly ILogger<AuthController> _logger;

    private readonly IAuthService _authService;

    public AuthController(ITokenService tokenService,
                          RoleManager<IdentityRole> roleManager,
                          UserManager<ApplicationUser> userManager,
                          IConfiguration config,
                          ILogger<AuthController> logger,
                          IAuthService authService,
                          IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _roleManager = roleManager;
        _userManager = userManager;
        _config = config;
        _logger = logger;
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
        var response = await _authService.CreateRole(roleName);
        if(response.Status == "Error")
           return StatusCode(StatusCodes.Status400BadRequest, response);
        
        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpPost]
    [Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var response = await _authService.AddUserToRole(email, roleName);
        if(response.Status == "Error")
           return StatusCode(StatusCodes.Status400BadRequest, response);

        return StatusCode(StatusCodes.Status200OK, response);
    }
}
