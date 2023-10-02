using AutoMapper;
using jonas.Application.Interfaces;
using jonas.Application.Services;
using jonas.Domain.Entities.Entities;
using jonas.Domain.Entities.Enums;
using jonas.Infrastructure.Database.Context;
using jonas.Infrastructure.Database.Transactions;
using jonas.MappingProfiles;
using jonas.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<UserMappingProfile>();
    cfg.AddProfile<RoleMappingProfile>();
    // Define other mappings here
});

// Add services to the container.
builder.Services.AddSingleton(mapperConfiguration.CreateMapper());
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserAccess", policy =>
        policy.RequireAssertion(context =>
        {
            return context.User.HasClaim(c =>
                (c.Type == ClaimTypes.Role &&
                (c.Value == UserRoles.User || c.Value == UserRoles.Admin)));
        }));
    options.AddPolicy("AdminAccess", policy =>
        policy.RequireClaim(ClaimTypes.Role, UserRoles.Admin));
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {

            //you can configure your custom policy
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});



// For global exception handling purposes
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            throw new BadHttpRequestException("Model not valid");
        };
    });

var app = builder.Build();

// Configure CORS to allow all origins
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());


app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
