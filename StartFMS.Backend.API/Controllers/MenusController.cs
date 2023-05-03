using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using StartFMS.Models.Backend;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StartFMS.Backend.API.Controllers
{

    [ApiController]
    [Route("api/user/menus")]
    //[Route("api/user/{id}/[controller]")]
    public class MenusController : ControllerBase
    {
        private readonly A00_BackendContext _BackendContext;
        public MenusController(A00_BackendContext BackendContext)
        {
            _BackendContext = BackendContext;
        }

        [HttpGet]
        public ActionResult<string> Get_MenuItms()
        {
            var menuItems = _BackendContext.S01MenuBasicSettings
                .OrderBy(m => m.DisplayOrder) // 依照 DisplayOrder 排序
                .ToList();

            var rootItems = menuItems.Where(m => m.ParentId == null).OrderBy(x=>x.DisplayOrder); // 取得根菜單項目

            foreach (var item in rootItems)
            {
                item.Children = menuItems
                    .Where(m => m.ParentId == item.Id)
                    .OrderBy(m => m.DisplayOrder) // 依照 DisplayOrder 排序
                    .ToList();
            }

            var json = JsonConvert.SerializeObject(rootItems, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 128,
            });

            return json;
        }
    }
}
