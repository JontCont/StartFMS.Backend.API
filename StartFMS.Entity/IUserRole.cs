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
        public IEnumerable<EF.UserRole>? GetRoleAll();
        public EF.UserRole? GetRole(Guid? id);

        public void AddUserRole(EF.UserRole? role);
        public void DeleteUserRole(Guid? id);
        public void UpdateUserRole(EF.UserRole? role);
    }

    public class UserRole : IUserRole
    {
        private StartFmsBackendContext _BackendContext { get; set; }
        private ILogger _Logger { get; set; }
        public UserRole(ILogger<UserRole> logger,StartFmsBackendContext context)
        {
            _Logger = logger;
            _BackendContext = context;
        }

        public EF.UserRole? GetRole(Guid? id)
        {
            return _BackendContext.UserRoles.Where(x => x.Id == id).FirstOrDefault();
        }

        public void AddUserRole(EF.UserRole? role)
        {
            _BackendContext.UserRoles.Add(role);
            _BackendContext.SaveChanges();
        }

        public void DeleteUserRole(Guid? id)
        {
            _BackendContext.UserRoles.Remove(GetRole(id));
            _BackendContext.SaveChanges();
        }

        public void UpdateUserRole(EF.UserRole? role)
        {
            _BackendContext.UserRoles.Update(role);
            _BackendContext.SaveChanges();
        }

        public IEnumerable<EF.UserRole>? GetRoleAll()
        {
            return _BackendContext.UserRoles.AsEnumerable();
        }
    }
}
