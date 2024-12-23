﻿using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StartFMS.EF;
using StartFMS.Extensions.Data;
using StartFMS.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StartFMS.Entity
{
    public interface IUsers
    {
        string ErrorMessage { get; }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Login(string userName, string password);

        /// <summary>
        /// 取得授權Token
        /// </summary>
        /// <returns></returns>
        string? GetAuthrizeToken();

        /// <summary>
        /// 取得使用者名稱
        /// </summary>
        /// <returns></returns>
        string? GetUserName();

        /// <summary>
        /// 取得使用者角色
        /// </summary>
        /// <returns></returns>
        string? GetUserRole();

        bool CreateAccount(UserRegistration models);
    }

    public class Users : IUsers
    {
        public string ErrorMessage => errorMessage;

        //區塊變數
        private string errorMessage { get; set; } = string.Empty;
        private Guid? userId { get; set; }
        private Guid? userRoleId { get; set; }
        private string Signing { get; set; }
        private string Issuer { get; set; }
        private string Audience { get; set; }

        //共用變數
        private StartFmsBackendContext _BackendContext { get; set; }
        private ILogger _Logger { get; set; }
        public Users(string signing, string issuer, string audience,
            ILogger<Users>? logger, StartFmsBackendContext context)
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
                    userRoleId = user.UserRoleId;
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

        public string? GetUserRole()
        {
            return _BackendContext.UserRoles
                .FirstOrDefault(x => x.Id == this.userRoleId)?
                .Name;
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
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
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


        public bool CreateAccount(UserRegistration models)
        {
            bool isFail = false;
            if (string.IsNullOrEmpty(models.Account) || string.IsNullOrEmpty(models.Password))
            {
                SetErrorMessage("帳號或密碼不可為空");
                return isFail;
            }

            if (_BackendContext.UserAccounts.Any(x => x.Account == models.Account))
            {
                SetErrorMessage("帳號已存在");
                return isFail;
            }

            if (_BackendContext.UserAccounts.Any(x => x.Email == models.Email))
            {
                SetErrorMessage("信箱已存在");
                return isFail;
            }

            if (!_BackendContext.UserRoles.Any(x => x.Name == "user"))
            {
                SetErrorMessage("身分無法成功建立，請聯絡管理員");
                return isFail;
            }

            UserAccount user = new UserAccount().SetValue(models);
            user.Id = Guid.NewGuid();
            user.UserRoleId = _BackendContext.UserRoles
                .Where(x => x.Name == "user")
                .Select(x => x.Id)
                .FirstOrDefault();
            _BackendContext.Add(user);
            _BackendContext.SaveChanges();

            return !isFail;
        }
    }

}
