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
            _context.UserAccounts.ExecuteDelete();
            _context.UserRoles.ExecuteDelete();
            _context.SystemCatalogItems.ExecuteDelete();
            _context.SaveChanges();
            //取得 initdata 資料夾底下所有的 csv 檔案
            var csvFiles = Directory.GetFiles("initdata", "*.csv");
            if (csvFiles.Length <= 0)
            {
                _context.Database.RollbackTransaction();
                return Ok("執行成功 (沒有 initdata 資料)");
            }
            var csvFilesByName = csvFiles
                .Select(x => new { Name = Path.GetFileNameWithoutExtension(x), Content = x })
                .OrderBy(x => x.Name);

            foreach (var files in csvFilesByName)
            {
                if (string.IsNullOrWhiteSpace(files.Name)) continue; // 檔案名稱為空白時跳過

                var csvData = System.IO.File.ReadAllText(files.Content);
                var csvReader = new CsvReader(new StringReader(csvData), new CsvConfiguration(CultureInfo.InvariantCulture));
                var records = csvReader.GetRecords<dynamic>().ToList();

                switch (files.Name.Split('_').LastOrDefault()!.ToEnum<SystemNames>())
                {
                    case SystemNames.UserRole:
                        foreach (var record in records)
                        {
                            var userRole = new UserRole
                            {
                                Id = Guid.TryParse(record.Id, out Guid format) ? format : throw new Exception("識別碼轉換失敗"),
                                Name = record.Name,
                                Description = record.Description,
                                IsEnabled = record.IsEnabled == "1",
                            };
                            _context.Entry(userRole).State = EntityState.Added;
                        }
                        _context.SaveChanges();
                        break;
                    case SystemNames.UserAccounts:
                        foreach (var record in records)
                        {
                            var userAccount = new UserAccount
                            {
                                Id = Guid.TryParse(record.Id, out Guid format) ? format : throw new Exception("識別碼轉換失敗"),
                                Account = record.Account,
                                Password = record.Password,
                                Name = record.Name,
                                Email = record.Email,
                                UserRoleId = Guid.TryParse(record.UserRoleId, out Guid UserRoleId) ? UserRoleId : throw new Exception("UserRoleId 轉換失敗"),
                                IsEnabled = record.IsEnabled == "1",
                            };
                            _context.Entry(userAccount).State = EntityState.Added;
                        }
                        _context.SaveChanges();

                        break;
                    case SystemNames.SystemCatalogItems:
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[SystemCatalogItems] ON");
                        foreach (var record in records)
                        {
                            var systemCatalogItem = new SystemCatalogItem
                            {
                                Id = int.TryParse(record.Id, out int identityId) ? identityId : throw new Exception("識別碼轉換失敗"),
                                MenuName = record.MenuName,
                                Description = record.Description,
                                DisplayOrder = int.TryParse(record.DisplayOrder, out int DisplayOrder) ? DisplayOrder : 0,
                                Url = record.Url,
                                Icon = record.Icon,
                                ParentId = int.TryParse(record.ParentId, out int ParentId) ? ParentId : null,
                                IsGroup = record.IsGroup == "1",
                                ImportAt = record.ImportAt
                            };
                            _context.Entry(systemCatalogItem).State = EntityState.Added;
                        }
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
