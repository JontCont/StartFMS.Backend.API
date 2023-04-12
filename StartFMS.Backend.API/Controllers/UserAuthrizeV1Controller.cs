using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Models.Backend;
using StartFMS.Models.Identity;
using StartFMS.Backend.Extensions;
using System.Net.Mail;
using System.Security.Cryptography;
using StartFMS.Backend.API.Dtos;

namespace StartFMS.Backend.API.Controllers;

public class JsonResult {
    public bool Success { get; set; } = false;
    public string Message { get; set; } = "";
    public string? Error { get; set; }
    public string? Token { get; set; }
}

[ApiController]
[AllowAnonymous]
[Route("api/identity/v1.0/")]
public class UserAuthrizeV1Controller : Controller {
    private readonly ILogger<UserAuthrizeV1Controller> _logger;
    private readonly A00_BackendContext _context;
    private readonly JwtHelpers _jwtHelpers;

    public UserAuthrizeV1Controller(
        ILogger<UserAuthrizeV1Controller> logger,
        A00_BackendContext backendContext, 
        JwtHelpers jwtHelpers) {
        _logger = logger;
        _context = backendContext;
        _jwtHelpers = jwtHelpers;
    }

    [HttpPost("Login")]
    public string PostFormIdentity([FromBody] LoginPost identity) {
        var User = _context.A00AccountUsers
            .Where(item => IsValidEmail(identity.Account) ? item.Email == identity.Account : item.UserName == identity.Account)
            .Where(item => item.PasswordHash.ToUpper() == identity.Password.ToUpper())
            .SingleOrDefault(); 

        if (User == null) {
            return "帳號密碼錯誤";
        }

        string resultToken = _jwtHelpers.GenerateToken(User.UserName);
        return resultToken;
    }//PostFormIdentity()

    [HttpGet(Name = "")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public string GetFormIdentity() {
        UsersAuthorize users = new UsersAuthorize(Request);
        if (users == null)
            return JsonConvert.SerializeObject(new JsonResult {
                Success = false,
                Error = "403 Forbidden",
                Message = "驗證身分失敗。"
            });
        return (users != null)
            ? JsonConvert.SerializeObject(new JsonResult { Success = true }, Formatting.None)
            : JsonConvert.SerializeObject(new JsonResult { Success = false }, Formatting.None);
    }//GetFormIdentity()



    static bool IsValidEmail(string email) {
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch {
            return false;
        }
    }
}
