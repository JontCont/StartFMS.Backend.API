using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StartFMS.Models.Backend;
using System.Data.Common;

namespace StartFMS.Backend.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/backend/sys/[controller]")]
public class SystemConfigSettingController : ControllerBase
{

    private readonly A00_BackendContext _BackendContext;
    public SystemConfigSettingController(A00_BackendContext BackendContext)
    {
        _BackendContext = BackendContext;
    }

    class columnsProp
    {
        public string name { get; set; }
        public string label { get; set;}
        public bool hidden { get; set; } = false;
    }

    [HttpGet]
    public ActionResult<string> Get_DataTable()
    {
        var result = _BackendContext.S10SystemConfigs;
        var columsList = new List<columnsProp>() {
            new columnsProp{ name = "ParName", label ="ParName" },
            new columnsProp{ name = "ParValue", label ="ParValue" },
            new columnsProp{ name = "ParMemo", label ="ParMemo" },
        };
        return JsonConvert.SerializeObject(new { 
            columns = columsList,
            data = result
        });
    }

    [HttpGet("{id}")]
    public ActionResult<S10SystemConfig> Get_MenuBasicSetting(string id)
    {
        var result = _BackendContext.S10SystemConfigs.Find(id);
        return (result == null) ? NotFound() : result;
    }

    [HttpPost]
    public ActionResult<S10SystemConfig> Post_MenuBasicSetting([FromBody] S10SystemConfig form)
    {
        _BackendContext.S10SystemConfigs.Add(form);
        _BackendContext.SaveChanges();
        return CreatedAtAction(nameof(Get_MenuBasicSetting), new { id = form.ParName }, form);
    }

    [HttpPut("{id}")]
    public ActionResult<S10SystemConfig> Put_MenuBasicSetting(string id, [FromBody] S10SystemConfig form)
    {
        if (id != form.ParName) { return BadRequest(); }
        _BackendContext.Entry(form).State = EntityState.Modified;

        try
        {
            _BackendContext.SaveChanges();
        }
        catch (DbException ex)
        {
            if(!_BackendContext.S10SystemConfigs.Any(x=>x.ParName == id))
            {
                return NotFound();
            }
            else
            {
                return StatusCode(500,"存取發生錯誤");
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete_MenuBasicSetting(Guid id)
    {
        var result = _BackendContext.S10SystemConfigs.Find(id);
        if (result == null) NotFound();
        _BackendContext.S10SystemConfigs.Remove(result);
        _BackendContext.SaveChanges();
        return NoContent();
    }
}
