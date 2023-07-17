using Microsoft.EntityFrameworkCore;
using StartFMS.Backend.API.Interface;
using StartFMS.Models.Backend;
using System.Security.Principal;

namespace StartFMS.Backend.API.Entity
{
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

        private void SetErrorMessage(string message)
        {
            errorMessage = message;
            _Logger.LogError(message);
        }
    }


}
