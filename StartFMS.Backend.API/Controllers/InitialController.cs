#pragma warning disable CS1591 // 遺漏公用可見類型或成員的 XML 註解

using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartFMS.Backend.Extensions;
using System.Globalization;

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
        try
        {
            var userRoles = _context.UserRoles.ToList();
            _context.Entry(userRoles).State = EntityState.Detached;

            var userAccounts = _context.UserAccounts.ToList();
            _context.Entry(userAccounts).State = EntityState.Detached;

            var systemItems = _context.SystemCatalogItems.ToList();
            _context.Entry(systemItems).State = EntityState.Detached;

            //取得 initdata 資料夾底下所有的 csv 檔案
            var csvFiles = Directory.GetFiles("initdata", "*.csv");

            foreach (var csvFile in csvFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(csvFile);
                if (string.IsNullOrWhiteSpace(fileName)) continue; // 檔案名稱為空白時跳過

                var csvData = System.IO.File.ReadAllText(csvFile);
                var csvReader = new CsvReader(new StringReader(csvData), new CsvConfiguration(CultureInfo.InvariantCulture));
                var records = csvReader.GetRecords<dynamic>().ToList();

                switch (fileName.ToEnum<SystemNames>())
                {
                    case SystemNames.UserRoles:
                        foreach (var record in records)
                        {
                            var userRole = new UserRole
                            {
                                Id = record.Id,
                                Name = record.Name,
                                Description = record.Description,
                                IsEnabled = record.IsEnabled,
                            };
                            _context.UserRoles.Add(userRole);
                        }

                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[UserRoles] ON");
                        _context.Entry(userRoles).State = EntityState.Added;
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[UserRoles] OFF");

                        break;
                    case SystemNames.UserAccounts:
                        foreach (var record in records)
                        {
                            var userAccount = new UserAccount
                            {
                                Id = record.Id,
                                Account = record.Account,
                                Password = record.Password,
                                Name = record.Name,
                                Email = record.Email,
                                UserRoleId = record.UserRoleId,
                                IsEnabled = record.IsEnabled,
                            };
                            _context.UserAccounts.Add(userAccount);
                        }

                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[UserAccounts] ON");
                        _context.Entry(userAccounts).State = EntityState.Added;
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[UserAccounts] OFF");

                        break;
                    case SystemNames.SystemCatalogItems:
                        foreach (var record in records)
                        {
                            var systemCatalogItem = new SystemCatalogItem
                            {
                                Id = record.Id,
                                MenuName = record.MenuName,
                                Description = record.Description,
                                DisplayOrder = record.DisplayOrder,
                                Url = record.Url,
                                Icon = record.Icon,
                                ParentId = record.ParentId,
                                IsGroup = record.IsGroup,
                                ImportAt = record.ImportAt
                            };
                            _context.SystemCatalogItems.Add(systemCatalogItem);
                        }

                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[SystemCatalogItems] ON");
                        _context.Entry(systemItems).State = EntityState.Added;
                        _context.SaveChanges();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[SystemCatalogItems] OFF");

                        break;
                }
            }
            _context.Database.CommitTransaction();
        }
        catch (Exception ex)
        {
            _context.Database.RollbackTransaction();
            return BadRequest(ex.Message);
        }
        return Ok("執行成功");
    }

}
