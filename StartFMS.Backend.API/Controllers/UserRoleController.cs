using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Backend.API.Dtos;
using StartFMS.Backend.Extensions;
using StartFMS.EF;
using StartFMS.Entity;

namespace StartFMS.Backend.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/sys")]
public class UserRoleController : Controller
{
    private readonly ILogger<UserRoleController> _logger;
    private readonly StartFmsBackendContext _context;
    private readonly IUserRole _roles;

    /// <summary>
    /// 初始化 <see cref="UserRoleController"/> 類別的新執行個體。
    /// </summary>
    /// <param name="logger">日誌記錄器。</param>
    /// <param name="users">使用者服務。</param>
    /// <param name="backendContext">後端內容。</param>
    /// <param name="jwtHelpers">JWT 輔助工具。</param>
    public UserRoleController(
        ILogger<UserRoleController> logger,
        IUserRole roles,
        StartFmsBackendContext backendContext)
    {
        _logger = logger;
        _roles = roles;
        _context = backendContext;
    }

    [HttpGet("role")]
    public IEnumerable<EF.UserRole>? GetRoleAll()
    {
        return _roles.GetRoleAll();      
    }

    [HttpGet("role/{id}")]
    public EF.UserRole? GetRole(Guid? id)
    {
        return _roles.GetRole(id);
    }   


}
