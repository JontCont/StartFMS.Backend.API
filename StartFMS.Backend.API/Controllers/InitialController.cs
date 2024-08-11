#pragma warning disable CS1591 // 遺漏公用可見類型或成員的 XML 註解

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartFMS.Backend.Extensions;
using StartFMS.EF;
using StartFMS.Entity;

namespace StartFMS.Backend.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/Initial/")]
public class InitialController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly StartFmsBackendContext _context;
    private readonly JwtHelpers _jwtHelpers;
    private readonly IUsers _users;

    /// <summary>
    /// 初始化 <see cref="LoginController"/> 類別的新執行個體。
    /// </summary>
    /// <param name="logger">日誌記錄器。</param>
    /// <param name="context"></param>
    /// <param name="users">使用者服務。</param>
    /// <param name="jwtHelpers">JWT 輔助工具。</param>
    public InitialController(ILogger<LoginController> logger, StartFmsBackendContext context, JwtHelpers jwtHelpers, IUsers users)
    {
        _logger = logger;
        _context = context;
        _jwtHelpers = jwtHelpers;
        _users = users;
    }

    [HttpGet("DataBase")]
    public IActionResult InitialDataBase()
    {
        if (string.IsNullOrEmpty(_context.Database.GetConnectionString()))
        {
            _logger.LogError("未設定資料庫連線字串 , Ex : connect string : {0}", _context.Database.GetConnectionString());
            return BadRequest($"未設定資料庫連線字串 , Ex : connect string :  {_context.Database.GetConnectionString()}");
        }

        if (!_context.Database.CanConnect())
        {
            _context.Database.EnsureCreated();
            return Ok("執行成功");
        }

        return Ok("資料庫已存在");
    }

    [HttpGet("Data")]
    public IActionResult InitialUsersData()
    {
        _context.Database.BeginTransaction();
        var userRoles = _context.UserRoles.ToList();
        _context.RemoveRange(userRoles);

        var userAccounts = _context.UserAccounts.ToList();
        _context.RemoveRange(userAccounts);

        var meuns = _context.SystemCatalogItems.ToList();
        _context.RemoveRange(meuns);

        // 檢查是否有資料
        List<UserRole> userRolesList = new List<UserRole>(){
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Description = "管理員",
                    IsEnabled = true,
                },
                new UserRole
                {
                    Id = Guid.NewGuid(),
                    Name = "User",
                    Description = "使用者",
                    IsEnabled = true,
                }
            };
        _context.UserRoles.AddRange(userRolesList);
        UserAccount userAccount = new UserAccount
        {
            Id = Guid.NewGuid(),
            Account = "admin",
            Password = "admin",
            Name = "管理員",
            Email = "admine@gmail.com",
            UserRoleId = userRolesList.FirstOrDefault(x => x.Name == "Admin")!.Id,
            IsEnabled = true,
        };
        _context.UserAccounts.Add(userAccount);

        _context.SystemCatalogItems.Add(new SystemCatalogItem()
        {
            MenuName = "系統管理",
            Icon = "fa fa-cogs",
            IsGroup = true,
            DisplayOrder = 1,
        });
        _context.SaveChanges();

        _context.SystemCatalogItems.Add(new SystemCatalogItem()
        {
            MenuName = "參數管理",
            Icon = "fa fa-cog",
            IsGroup = false,
            DisplayOrder = 1,
            ParentId = _context.SystemCatalogItems.FirstOrDefault(x => x.MenuName == "系統管理")!.Id,
            Url = "/system/parameter",
            ImportAt = "System/SystemParameter/SystemParameter",
        });
        _context.SystemCatalogItems.Add(new SystemCatalogItem()
        {
            MenuName = "目錄設定",
            Icon = "fa fa-cog",
            IsGroup = false,
            DisplayOrder = 2,
            ParentId = _context.SystemCatalogItems.FirstOrDefault(x => x.MenuName == "系統管理")!.Id,
            Url = "/system/meum-setting",
            ImportAt = "System/SystemParameter/SystemParameter",
        });
        _context.SaveChanges();

        _context.Database.CommitTransaction();
        return Ok("執行成功");
    }

}
