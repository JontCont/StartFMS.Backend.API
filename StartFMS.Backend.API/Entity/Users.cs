﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StartFMS.Backend.API.Interface;
using StartFMS.Backend.Extensions;
using StartFMS.Models.Backend;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace StartFMS.Backend.API.Entity;

public class UserManager : IUsers
{
    public string ErrorMessage => errorMessage;

    //區塊變數
    private string errorMessage { get; set; } = string.Empty;
    private Guid? userId { get; set; }

    //共用變數
    private A00_BackendContext _BackendContext { get; set; }
    private ILogger _Logger { get; set; }
    public UserManager(ILogger<UserManager>? logger, A00_BackendContext context)
    {
        _Logger = logger;
        _BackendContext = context;
    }

    public bool Login(string userName, string password)
    {
        bool isFail = false;
        if (userName == null || password == null)
        {
            isFail = true;
            SetErrorMessage("Username or password cannot be null.");
        }

        if (!_BackendContext.Database.CanConnect())
        {
            isFail = true;
            SetErrorMessage("資料庫連線失效");
        }
        else
        {
            var user = _BackendContext.A00Accounts
                .Where(item => item.Account == userName)
                .Where(item => item.Password == password)
                .SingleOrDefault();

            if (user == null)
            {
                isFail = true;
                SetErrorMessage("登入失敗");
            }
            else
            {
                userId = user.EmployeeId;
            }
        }

        return isFail;
    }

    public string? GetUserName()
    {
        if (userId == null)
        {
            throw new Exception("尚未登入");
        }

        return _BackendContext.A00Accounts
            .FirstOrDefault(x => x.EmployeeId == this.userId)?
            .Name;
    }


    public string? GetAuthrizeToken()
    {
        if (userId == null)
        {
            throw new Exception("尚未登入");
        }

        // 使用者身分資料
        var user = _BackendContext.A00Accounts
            .FirstOrDefault(x => x.EmployeeId == this.userId);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Account),
                new Claim("FullName", user.Name),
                new Claim(JwtRegisteredClaimNames.NameId, user.EmployeeId.ToString()),
                new Claim("EmployeeId", user.EmployeeId.ToString())
            };

        //腳色
        var role = from a in _BackendContext.A00Roles
                   where a.EmployeeId == user.EmployeeId
                   select a;

        foreach (var temp in role)
        {
            claims.Add(new Claim(ClaimTypes.Role, temp.Name));
        }

        return GenerateToken(claims);
    }


    private void SetErrorMessage(string message)
    {
        errorMessage = message;
        _Logger.LogError(message);
    }


    /// <summary>
    /// 從 BDP080 取得資料登入 
    /// </summary>
    /// <param name="userAutos">使用者驗證</param>
    /// <param name="expireMinutes">時效</param>
    /// <returns></returns>
    private string GenerateToken(List<Claim> claims, int expireMinutes = 30)
    {
        var userClaimsIdentity = new ClaimsIdentity(claims);
        // Create a SymmetricSecurityKey for JWT Token signatures
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ASDZXASDHAUISDHASDOHAHSDUAHDS"));

        // HmacSha256 MUST be larger than 128 bits, so the key can't be too short. At least 16 and more characters.
        // https://stackoverflow.com/questions/47279947/idx10603-the-algorithm-hs256-requires-the-securitykey-keysize-to-be-greater
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Create SecurityTokenDescriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "test",
            // Audience = Issuer, // Sometimes you don't have to define Audience.
            // NotBefore = DateTime.Now, // Default is DateTime.Now
            // IssuedAt = DateTime.Now, // Default is DateTime.Now
            Subject = userClaimsIdentity,
            Expires = DateTime.Now.AddMinutes(expireMinutes),
            SigningCredentials = signingCredentials
        };

        // Generate a JWT securityToken, than get the serialized Token result (string)
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var serializeToken = tokenHandler.WriteToken(securityToken);

        return serializeToken;
    }


}
