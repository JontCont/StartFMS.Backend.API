using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartFMS.Backend.API.Dtos;
using StartFMS.Backend.Extensions;
using StartFMS.Models.Backend;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using StartFMS.Extensions.Configuration;

namespace StartFMS.Backend.API.Controllers;

public class LoginController : Controller
{

    private readonly ILogger<UserAuthrizeV1Controller> _logger;
    private readonly A00_BackendContext _context;
    private readonly JwtHelpers _jwtHelpers;

    public LoginController(
        ILogger<UserAuthrizeV1Controller> logger,
        A00_BackendContext backendContext,
        JwtHelpers jwtHelpers)
    {
        _logger = logger;
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
        var user = (from a in _context.A00Accounts
                    where a.Account == value.Account
                    && a.Password == value.Password
                    select a).SingleOrDefault();

        if (user == null)
        {
            return "帳號密碼錯誤";
        }
        else
        {

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Account),
                    new Claim("FullName", user.Name),
                    new Claim(JwtRegisteredClaimNames.NameId, user.EmployeeId.ToString()),
                    new Claim("EmployeeId", user.EmployeeId.ToString())
                };

            var role = from a in _context.A00Roles
                       where a.EmployeeId == user.EmployeeId
                       select a;

            foreach (var temp in role)
            {
                claims.Add(new Claim(ClaimTypes.Role, temp.Name));
            }

            var token = _jwtHelpers.GenerateToken(claims);
            return token;
        }
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
