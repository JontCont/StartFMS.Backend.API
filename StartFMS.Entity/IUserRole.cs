using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StartFMS.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StartFMS.Entity
{
    public interface IUserRole
    {
        public IEnumerable<UserRole>? GetRoleAll();
        public UserRole? GetRole(Guid? id);

        public void AddUserRole(UserRole? role);
        public void DeleteUserRole(Guid? id);
        public void UpdateUserRole(UserRole? role);
    }

    public class UserRoles : IUserRole
    {
        private StartFmsBackendContext _BackendContext { get; set; }
        private ILogger _Logger { get; set; }
        public UserRoles(ILogger<UserRoles> logger,StartFmsBackendContext context)
        {
            _Logger = logger;
            _BackendContext = context;
        }

        public UserRole? GetRole(Guid? id)
        {
            return _BackendContext.UserRoles.Where(x => x.Id == id).FirstOrDefault();
        }

        public void AddUserRole(UserRole? role)
        {
            _BackendContext.UserRoles.Add(role);
            _BackendContext.SaveChanges();
        }

        public void DeleteUserRole(Guid? id)
        {
            _BackendContext.UserRoles.Remove(GetRole(id));
            _BackendContext.SaveChanges();
        }

        public void UpdateUserRole(UserRole? role)
        {
            _BackendContext.UserRoles.Update(role);
            _BackendContext.SaveChanges();
        }

        public IEnumerable<UserRole>? GetRoleAll()
        {
            return _BackendContext.UserRoles.AsEnumerable();
        }
    }
}
