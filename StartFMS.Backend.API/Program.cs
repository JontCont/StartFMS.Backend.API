using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StartFMS.Backend.API.Entity;
using StartFMS.Backend.API.Filters;
using StartFMS.Backend.API.Interface;
using StartFMS.Backend.API.Migrations;
using StartFMS.Backend.Extensions;
using StartFMS.Extensions.Configuration;
using StartFMS.Models.Backend;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    //logging.AddFile("app.log");
});
var config = Config.GetConfiguration<Program>(); //加入設定檔
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add core content
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
        });

    options.AddPolicy("AnotherPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

builder.Services.AddControllers(services =>
{
    services.Filters.Add(typeof(AuthorizationFilter));
    services.Filters.Add(typeof(LogActionFilters));
    services.Filters.Add(typeof(LogExceptionFilter));
});


builder.Services.AddDbContext<A00_BackendContext>(content =>
{
    content.UseSqlServer(config.GetConnectionString("Develop"), b => {
        b.MigrationsAssembly("StartFMS.Backend.API");
    });
});

// 使用 ActivatorUtilities.CreateInstance 提供設定值並註冊 UserManager
builder.Services.AddScoped<IUsers>(provider =>
{
    // 在這裡從組態文件中取得相關的設定值
    var signing = config.GetValue<string>("JwtSettings:KEY");
    var issuer = config.GetValue<string>("JwtSettings:Issuer");
    var audience = config.GetValue<string>("JwtSettings:Audience");

    return ActivatorUtilities.CreateInstance<UserManager>(provider, signing, issuer, audience,
                                                          provider.GetRequiredService<A00_BackendContext>(),
                                                          provider.GetRequiredService<ILogger<UserManager>>());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Start Five Minutes Backend API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization"
    });

    c.AddSecurityRequirement(
    new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

JwtHelpers jwtHelpers = new JwtHelpers()
{
    Signing = config.GetValue<string>("JwtSettings:KEY"),
    Issuer = config.GetValue<string>("JwtSettings:Issuer"),
    Audience = config.GetValue<string>("JwtSettings:Audience"),
};
builder.Services.AddSingleton(jwtHelpers);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
