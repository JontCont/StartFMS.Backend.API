using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StartFMS.Backend.API.Controllers.Users;
using StartFMS.Backend.API.Dtos;
using StartFMS.Backend.API.Interface;
using StartFMS.Backend.Extensions;
using StartFMS.Models.Backend;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StartFMS.Backend.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth/v1.0/Login/")]
public class LoginController : Controller
{
    private readonly ILogger<UserAuthrizeV1Controller> _logger;
    private readonly A00_BackendContext _context;
    private readonly JwtHelpers _jwtHelpers;
    private readonly IUsers _users;
    public LoginController(
        ILogger<UserAuthrizeV1Controller> logger,
        IUsers users,
        A00_BackendContext backendContext,
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
        var user = _context.A00Accounts
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
                    new Claim("FullName", user.Name),
                    new Claim("EmployeeId", user.EmployeeId.ToString())
                };

            var role = from a in _context.A00Roles
                       where a.EmployeeId == user.EmployeeId
                       select a;

            foreach (var temp in role)
            {
                claims.Add(new Claim(ClaimTypes.Role, temp.Name));
            }

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
