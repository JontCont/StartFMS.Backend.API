namespace StartFMS.Backend.API.Interface
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
    }
}
