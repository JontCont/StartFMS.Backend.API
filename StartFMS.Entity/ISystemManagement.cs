using Microsoft.Extensions.Logging;
using StartFMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartFMS.Entity
{
    public interface ISystemManagement
    {
        //系統參數
        IEnumerable<SystemParameter>? GetSystemParameters();
        IEnumerable<SystemParameter>? GetSystemParameters(string name);
        SystemParameter? GetSystemParameters(int id);
        SystemParameter SaveSystemParameters(SystemParameter? systemParameter);
        void DeleteSystemParameters(int id);
    }

    public class SystemParameters : ISystemManagement
    {
        private StartFmsBackendContext _BackendContext { get; set; }
        private ILogger _Logger { get; set; }
        public SystemParameters(ILogger<SystemParameters> logger, StartFmsBackendContext context)
        {
            _Logger = logger;
            _BackendContext = context;
        }

        public IEnumerable<SystemParameter>? GetSystemParameters()
        {
            return _BackendContext.SystemParameters.AsEnumerable();
        }

        public IEnumerable<SystemParameter>? GetSystemParameters(string name)
        {
            return _BackendContext.SystemParameters.Where(x => x.Name == name).AsEnumerable();
        }

        public SystemParameter? GetSystemParameters(int id)
        {
            return _BackendContext.SystemParameters.FirstOrDefault(x => x.Id == id);
        }

        public SystemParameter SaveSystemParameters(SystemParameter? systemParameter)
        {
            if (systemParameter == null)
            {
                _Logger.LogError("系統參數為空");
                throw new ArgumentNullException(nameof(systemParameter));
            }
            if (systemParameter.Id > 0)
            {
                _BackendContext.SystemParameters.Update(systemParameter);

            }
            else
            {
                _BackendContext.SystemParameters.Add(systemParameter);
            }
            _BackendContext.SaveChanges();
            return systemParameter;
        }

        public void DeleteSystemParameters(int id)
        {
            _BackendContext.SystemParameters.Remove(GetSystemParameters(id));
            _BackendContext.SaveChanges();
        }

    }
}
