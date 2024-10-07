using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyCleanArchitectureApp.Core.Interfaces;
using MyCleanArchitectureApp.Application.Services;
using MyCleanArchitectureApp.Infrastructure;
using MyCleanArchitectureApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration settings
var secretKey = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
// Add services to the container
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program)); // Register AutoMapper
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Register UnitOfWork
builder.Services.AddScoped<CountryService>();// Register CountryService
builder.Services.AddScoped<StateService>();// Register CountryService
builder.Services.AddScoped<ITokenService>(provider => new TokenService(secretKey, issuer, audience));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Use authentication middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
