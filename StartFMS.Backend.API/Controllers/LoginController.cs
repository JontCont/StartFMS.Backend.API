using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Backend.API.Controllers.Users;
using StartFMS.Backend.API.Dtos;
using StartFMS.Backend.Extensions;
using StartFMS.EF;
using StartFMS.Entity;
using System.Security.Claims;

namespace StartFMS.Backend.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth/v1.0/Login/")]
public class LoginController : Controller
{
    private readonly ILogger<UserAuthrizeV1Controller> _logger;
    private readonly StartFmsBackendContext _context;
    private readonly JwtHelpers _jwtHelpers;
    private readonly IUsers _users;
    public LoginController(
        ILogger<UserAuthrizeV1Controller> logger,
        IUsers users,
        StartFmsBackendContext backendContext,
        JwtHelpers jwtHelpers)
    {
        _logger = logger;
        _users = users;
        _context = backendContext;
        _jwtHelpers = jwtHelpers;
    }


    [HttpPost]
    public string PostFormIdentity([FromBody] LoginPost identity)
    {
        var user = _context.UserAccounts
            .Where(item =>  item.Account == identity.Account)
            .Where(item => item.Password == identity.Password)
            .SingleOrDefault();

        if (user == null)
        {
            return "帳號密碼錯誤";
        }
        else
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Account),
                    new Claim("FullName", user.Account),
                };

            claims.Add(new Claim(ClaimTypes.Role, "admin"));

            var authProperties = new AuthenticationProperties
            {
                // ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(2)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return "ok";
        }
    }//PostFormIdentity()

    [HttpPost("jwtLogin")]
    public string jwtLogin(LoginPost value)
    {
        if (!_users.Login(value.Account, value.Password)) {
            var msg = _users.ErrorMessage;

            return JsonConvert.SerializeObject(new
            {
                success = false,
                message = msg,
            });
        }

        var userName = _users.GetUserName();
        var token = _users.GetAuthrizeToken();

        return JsonConvert.SerializeObject(new
        {
            success = true,
            token = token,
            user = userName,
        });
    }

    [HttpDelete]
    public void logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("NoLogin")]
    public string noLogin()
    {
        return "未登入";
    }

    [HttpGet("NoAccess")]
    public string noAccess()
    {
        return "沒有權限";
    }

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
