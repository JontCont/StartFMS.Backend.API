namespace StartFMS.Backend.API.Interface
{
    public interface IUsers
    {
        bool Login(string userName, string password);


        string? GetUserName();
    }
}
