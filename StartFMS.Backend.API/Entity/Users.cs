using Microsoft.IdentityModel.Tokens;
using StartFMS.Backend.API.Interface;
using StartFMS.EF;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StartFMS.Backend.API.Entity;

public class UserManager : IUsers
{
    public string ErrorMessage => errorMessage;

    //區塊變數
    private string errorMessage { get; set; } = string.Empty;
    private Guid? userId { get; set; }
    private string Signing { get; set; }
    private string Issuer { get; set; }
    private string Audience { get; set; }

    //共用變數
    private StartFmsBackendContext _BackendContext { get; set; }
    private ILogger _Logger { get; set; }
    public UserManager(string signing, string issuer, string audience,
        ILogger<UserManager>? logger, StartFmsBackendContext context)
    {
        Signing = signing;
        Issuer = issuer;
        Audience = audience;

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
            var user = _BackendContext.UserAccounts
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
                userId = user.Id;
            }
        }

        return !isFail;
    }

    public string? GetUserName()
    {
        if (userId == null)
        {
            throw new Exception("尚未登入");
        }

        return _BackendContext.UserAccounts
            .FirstOrDefault(x => x.Id == this.userId)?
            .Account;
    }


    public string? GetAuthrizeToken()
    {
        if (userId == null)
        {
            throw new Exception("尚未登入");
        }

        // 使用者身分資料
        var user = _BackendContext.UserAccounts
            .FirstOrDefault(x => x.Id == this.userId);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Account),
                new Claim("FullName", user.Name),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            };

        //腳色
        claims.Add(new Claim(ClaimTypes.Role, "admin"));
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
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Signing));

        // HmacSha256 MUST be larger than 128 bits, so the key can't be too short. At least 16 and more characters.
        // https://stackoverflow.com/questions/47279947/idx10603-the-algorithm-hs256-requires-the-securitykey-keysize-to-be-greater
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Create SecurityTokenDescriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = Issuer,
            Audience = Audience, // Sometimes you don't have to define Audience.
            NotBefore = DateTime.Now, // Default is DateTime.Now
            IssuedAt = DateTime.Now, // Default is DateTime.Now
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
