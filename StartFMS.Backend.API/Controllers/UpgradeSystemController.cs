using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using StartFMS.Models.Backend;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace StartFMS.Backend.API.Controllers
{
    //[AllowAnonymous]
    [ApiController]
    [Route("api/sys/up/")]
    public class UpgradeSystemController : ControllerBase
    {
        private readonly A00_BackendContext _BackendContext;

        public UpgradeSystemController(A00_BackendContext BackendContext)
        {
            _BackendContext = BackendContext;
        }

        [HttpGet("version")]
        public string Get_DataBase_Version()
        {
            var data =_BackendContext.Database.GetAppliedMigrations();
            var json = JsonConvert.SerializeObject(data);
            return json;
        }

        [HttpGet("create")]
        public async Task<ActionResult<string>> CreateDataBase()
        {
            bool status = await _BackendContext.Database.EnsureCreatedAsync();
            var json = JsonConvert.SerializeObject(new
            {
                success = status,
                message = status ? "完成更新" : "更新失敗"
            });
            return json;
        }


        [HttpGet("update")]
        public async Task<ActionResult<string>> UpdateDataBase()
        {
            await _BackendContext.Database.MigrateAsync();
            var json = JsonConvert.SerializeObject(new
            {
                success = true,
                message = "完成更新"
            });
            return json;
        }
    }
}
