using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StartFMS.Backend.API.Dtos;
using StartFMS.Backend.Extensions;
using StartFMS.EF;
using StartFMS.Entity;

namespace StartFMS.Backend.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/user/menu")]
public class UserCatalogController : Controller
{
    private readonly ILogger<UserCatalogController> _logger;
    private readonly StartFmsBackendContext _context;

    /// <summary>
    /// 初始化 <see cref="UserCatalogController"/> 類別的新執行個體。
    /// </summary>
    /// <param name="logger">日誌記錄器。</param>
    /// <param name="users">使用者服務。</param>
    /// <param name="backendContext">後端內容。</param>
    /// <param name="jwtHelpers">JWT 輔助工具。</param>
    public UserCatalogController(
        ILogger<UserCatalogController> logger,
        StartFmsBackendContext backendContext)
    {
        _logger = logger;
        _context = backendContext;
    }

    [HttpGet("role")]
    public string GetRoleAll()
    {
        var menuItems = _context.SystemCatalogItems
       .OrderBy(m => m.DisplayOrder) // 依照 DisplayOrder 排序
       .ToList();

        var rootItems = menuItems
            .Where(m => m.ParentId == null)
            .OrderBy(x => x.DisplayOrder); // 取得根菜單項目

        foreach (var item in rootItems)
        {
            item.Children = menuItems
                .Where(m => m.ParentId == item.Id)
                .OrderBy(m => m.DisplayOrder) // 依照 DisplayOrder 排序
                .ToList();
        }

        var json = JsonConvert.SerializeObject(rootItems, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            MaxDepth = 128,
        });

        return json;
    }

}
