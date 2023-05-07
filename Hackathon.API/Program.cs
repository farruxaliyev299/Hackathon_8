using FluentValidation.AspNetCore;
using Hackathon.Application.Validations;
using Hackathon.Infrastructure.Persistence;
using Hackathon.Infrastructure.Persistence.Identity.Models;
using Hackathon.Infrastructure.Persistence.Initialize;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbInitialize, DbInitialize>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Hackathon.Application.Commands.PostCreditOrderCommand).Assembly);
});

builder.Services.AddDbContext<HackathonDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssembly(typeof(CreditApplicationValidation).Assembly);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(x =>
{
    x.Password.RequireUppercase = false;
    x.Password.RequireLowercase = false;
    x.Password.RequireDigit = false;
    x.Password.RequiredLength = 2;
    x.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<HackathonDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidIssuer = "https://localhost:7261",
                ValidAudience = "https://localhost:7261",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secure_key"))
            };
        });

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitialize>();
    dbInitializer.Initialize();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
