using MainBlog.Context;
using MainBlog.DTOs.Mappins;
using MainBlog.Exceptions;
using MainBlog.Logging;
using MainBlog.Models;
using MainBlog.Repository;
using MainBlog.Services.AuthenticationsServices;
using MainBlog.IRepository;
using MainBlog.IService;
using MainBlog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Ativa os constroladores da API
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; })
                .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the database context to the DI container.
string? mysqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the database context as a service and configure it to use MySQL.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(mysqlConnection).LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(DTOMappingProfile));

// --- Configura��es de Seguran�a

// Recupera a chave secreta da configura��o ou lan�a uma exce��o se n�o estiver definida
var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Invalid secret key!");
// Adiciona servi�os de autentica��o ao cont�iner de inje��o de depend�ncia
builder.Services.AddAuthentication(options =>
{
    // Define o esquema de autentica��o padr�o para JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Define o esquema de desafio padr�o para JWT Bearer
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    // Habilita a persist�ncia do token depois da autentica��o
    options.SaveToken = true;
    // Define se a metadata HTTPS � necess�ria na autentica��o. False para desenvolvimento local
    options.RequireHttpsMetadata = false;
    // Par�metros de valida��o do token
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // Habilita a valida��o do emissor do token
        ValidateIssuer = true,
        // Habilita a valida��o da audi�ncia do token
        ValidateAudience = true,
        // Habilita a valida��o do tempo de vida do token
        ValidateLifetime = true,
        // Habilita a valida��o da chave de assinatura do emissor
        ValidateIssuerSigningKey = true,
        // Define o desvio de tempo permitido para a valida��o de tempo de vida do token
        ClockSkew = TimeSpan.Zero,
        // Recupera a audi�ncia v�lida da configura��o
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        // Recupera o emissor v�lido da configura��o
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        // Define a chave de assinatura do emissor para a valida��o do token
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey))
    };
});
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("RequireSuperAdministratorRole",
                      policy => policy.RequireRole("SUPERADMIN"));
    options.AddPolicy("RequireAdministratorRole",
               policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("RequireUserRole",
               policy => policy.RequireRole("USER"));
});

// --- Fim das Configura��es de Seguran�a

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200") 
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}
app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
