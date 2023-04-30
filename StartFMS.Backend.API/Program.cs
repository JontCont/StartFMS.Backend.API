using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using StartFMS.Backend.API.Filters;
using StartFMS.Backend.Extensions;
using StartFMS.Extensions.Configuration;
using StartFMS.Models.Backend;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
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

builder.Services.AddControllers(services => {
    services.Filters.Add(typeof(AuthorizationFilter));
    services.Filters.Add(typeof(LogActionFilters));
    services.Filters.Add(typeof(LogExceptionFilter));
});


//builder.Services.AddDbContext<A00_BackendContext>(content => {
//    content.UseSqlServer(config.GetConnectionString("Develop"));
//});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Start Five Minutes Backend API", Version = "v1" });
});

JwtHelpers jwtHelpers = new JwtHelpers()
{
    Signing = config.GetValue<string>("JwtSettings:KEY"),
    Issuer = config.GetValue<string>("JwtSettings:Issuer"),
    Audience = config.GetValue<string>("JwtSettings:Audience"),
};
builder.Services.AddSingleton<JwtHelpers>(jwtHelpers);


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
