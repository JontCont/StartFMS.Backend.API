using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using StartFMS.Backend.API.Extensions;
using StartFMS.Backend.API.Filters;
using StartFMS.Backend.Extensions;
using StartFMS.EF;
using StartFMS.Entity;
using StartFMS.Extensions.Configuration;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders().AddConsole();

var config = builder.Configuration
    .AddJsonFile(path: $"appsettings.{builder.Environment.EnvironmentName}.json")
    .Build(); //加入設定檔

var test = config.GetConnectionString("Develop");

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
    services.Filters.Add(typeof(ApiResultFilter));
    services.Filters.Add(typeof(AuthorizationFilter));
    services.Filters.Add(typeof(LogActionFilters));
    services.Filters.Add(typeof(LogExceptionFilter));
});

//builder.Services.AddNe


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 128;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<StartFmsBackendContext>(content =>
{
    content.UseSqlServer(config.GetConnectionString("Develop"), b =>
    {
        b.MigrationsAssembly("StartFMS.Backend.API");
    });
});

// 使用 ActivatorUtilities.CreateInstance 提供設定值並註冊 UserManager
// 在這裡從組態文件中取得相關的設定值
var signing = config.GetValue<string>("JwtSettings:KEY");
var issuer = config.GetValue<string>("JwtSettings:Issuer");
var audience = config.GetValue<string>("JwtSettings:Audience");
builder.Services.AddScopedForInterface<IUsers, Users>(signing, issuer, audience);
builder.Services.AddScopedForInterface<IUserRole, UserRoles>();
builder.Services.AddScopedForInterface<ISystemManagement, SystemParameters>();



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Start Five Minutes Backend API",
        Version = "v1"
    });

    // 讀取 XML 檔案產生 API 說明
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

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


