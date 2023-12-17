
namespace StartFMS.Backend.API.Controllers
{
    public interface IUserRoleController
    {
        string GetRole(Guid? id);
        string GetRoleAll();
    }
}