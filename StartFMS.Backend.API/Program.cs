using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StartFMS.Backend.API.Filters;
using StartFMS.Backend.Extensions;
using StartFMS.EF;
using StartFMS.Entity;
using StartFMS.Extensions.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddConsole();
var config = Config.GetConfiguration<Program>(); //加入設定檔

builder.Services.AddControllers();
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

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // 添加這一行
    });


builder.Services.AddDbContext<StartFmsBackendContext>(content =>
{
    content.UseSqlServer(config.GetConnectionString("Develop"), b =>
    {
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

    return ActivatorUtilities.CreateInstance<Users>(provider, signing, issuer, audience,
                                                          provider.GetRequiredService<StartFmsBackendContext>(),
                                                          provider.GetRequiredService<ILogger<Users>>());
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

/// <summary>
/// Initializes a new instance of the JwtHelpers class with the specified signing key, issuer, and audience.
/// </summary>
/// <param name="signing">The signing key used to sign the JWT.</param>
/// <param name="issuer">The issuer of the JWT.</param>
/// <param name="audience">The audience of the JWT.</param>
JwtHelpers jwtHelpers = new JwtHelpers()
{
    Signing = config.GetValue<string>("JwtSettings:KEY") ?? "",
    Issuer = config.GetValue<string>("JwtSettings:Issuer") ?? "",
    Audience = config.GetValue<string>("JwtSettings:Audience") ?? "",
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
