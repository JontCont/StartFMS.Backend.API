namespace StartFMS.Backend.API.Interface
{
    public interface IUsers
    {
        string ErrorMessage { get; }

        bool Login(string userName, string password);

        string? GetAuthrizeToken();
        string? GetUserName();
    }
}
