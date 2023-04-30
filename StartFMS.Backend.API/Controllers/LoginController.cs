using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartFMS.Backend.API.Dtos;
using StartFMS.Backend.Extensions;
using StartFMS.Models.Backend;

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
        var User = _context.A00AccountUsers
            .Where(item => IsValidEmail(identity.Account) ? item.Email == identity.Account : item.UserName == identity.Account)
            .Where(item => item.PasswordHash.ToUpper() == identity.Password.ToUpper())
            .SingleOrDefault();

        if (User == null)
        {
            return "帳號密碼錯誤";
        }

        string resultToken = _jwtHelpers.GenerateToken(User.UserName);
        return resultToken;
    }//PostFormIdentity()

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
