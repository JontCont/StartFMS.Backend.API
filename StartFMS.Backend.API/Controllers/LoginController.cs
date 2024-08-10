using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Backend.API.Dtos;
using StartFMS.Backend.Extensions;
using StartFMS.EF;
using StartFMS.Entity;
using System.Net;

namespace StartFMS.Backend.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth")]
public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly StartFmsBackendContext _context;
    private readonly JwtHelpers _jwtHelpers;
    private readonly IUsers _users;

    /// <summary>
    /// 初始化 <see cref="LoginController"/> 類別的新執行個體。
    /// </summary>
    /// <param name="logger">日誌記錄器。</param>
    /// <param name="users">使用者服務。</param>
    /// <param name="backendContext">後端內容。</param>
    /// <param name="jwtHelpers">JWT 輔助工具。</param>
    public LoginController(
        ILogger<LoginController> logger,
        IUsers users,
        StartFmsBackendContext backendContext,
        JwtHelpers jwtHelpers)
    {
        _logger = logger;
        _users = users;
        _context = backendContext;
        _jwtHelpers = jwtHelpers;
    }

    /// <summary>
    /// 處理 JWT 登入請求。
    /// </summary>
    /// <param name="value">登入的 POST 資料。</param>
    /// <returns>包含登入結果的 JSON 回應。</returns>
    /// <remarks>包含登入結果的 JSON 回應。</remarks>
    [HttpPost("Login")]
    public IActionResult jwtLogin(LoginPost value)
    {
        if (!_users.Login(value.Account, value.Password))
        {
            var msg = _users.ErrorMessage;
            return Unauthorized(new RetrunJson
            {
                Data = null,
                HttpCode = (int)HttpStatusCode.Unauthorized,
                ErrorMessage = msg
            });
        }

        var userName = _users.GetUserName();
        var token = _users.GetAuthrizeToken();

        return Ok(new
        {
            UserName = userName,
            Token = token
        });
    }

    /// <summary>
    /// 處理登出請求。
    /// </summary>
    [HttpDelete("Login")]
    public void logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpPost("SignUp")]
    public IActionResult CreateUsers(LoginPost value)
    {
        if (!_users.Login(value.Account, value.Password))
        {
            var msg = _users.ErrorMessage;
            return Unauthorized(new RetrunJson
            {
                Data = null,
                HttpCode = (int)HttpStatusCode.Unauthorized,
                ErrorMessage = msg
            });
        }

        var userName = _users.GetUserName();
        var token = _users.GetAuthrizeToken();

        return Ok(new
        {
            UserName = userName,
            Token = token
        });
    }
    /// <summary>
    /// 處理 "NoLogin" 請求。
    /// </summary>
    /// <returns>回應訊息，指示使用者未登入。</returns>
    [HttpGet("NoLogin")]
    public string noLogin()
    {
        return "未登入";
    }

    /// <summary>
    /// 處理 "NoAccess" 請求。
    /// </summary>
    /// <returns>回應訊息，指示使用者無權限。</returns>
    [HttpGet("NoAccess")]
    public string noAccess()
    {
        return "沒有權限";
    }

    /// <summary>
    /// 檢查給定的電子郵件地址是否有效。
    /// </summary>
    /// <param name="email">要驗證的電子郵件地址。</param>
    /// <returns>如果電子郵件地址有效則為 true，否則為 false。</returns>
    static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
