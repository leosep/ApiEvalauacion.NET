using ApiEvaluacion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiEvaluacion.Application.Services;
using ApiEvaluacion.Application.Interfaces;
using ApiEvaluacion.Application;
using ApiEvaluacion.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Servicios 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("ApiEvaluacionDb"));
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// API externo
builder.Services.Configure<ExternalApiSettings>(builder.Configuration.GetSection("ExternalApis"));
builder.Services.AddHttpClient<IPostsRepository, PostsRepository>();

builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddTransient<IValidator<ApiEvaluacion.Application.DTOs.RegisterRequest>, ApiEvaluacion.Application.DTOs.RegisterRequestValidator>();

var app = builder.Build();

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ApiEvaluacion.Presentation.Middleware.ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
