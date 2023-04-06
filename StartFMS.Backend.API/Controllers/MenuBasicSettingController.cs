using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartFMS.Models.Backend;
using System.Data.Common;

namespace StartFMS.Backend.API.Controllers;


[ApiController]
[Route("api/user/[controller]")]
public class MenuBasicSettingController : ControllerBase
{

    private readonly A00_BackendContext _BackendContext;
    public MenuBasicSettingController(A00_BackendContext BackendContext)
    {
        _BackendContext = BackendContext;
    }

    [HttpGet("{id}")]
    public ActionResult<S01MenuBasicSetting> Get_MenuBasicSetting(Guid id)
    {
        var result = _BackendContext.S01MenuBasicSettings.Find(id);
        return (result == null) ? NotFound() : result;
    }

    [HttpPost]
    public ActionResult<S01MenuBasicSetting> Post_MenuBasicSetting([FromBody] S01MenuBasicSetting form)
    {
        _BackendContext.S01MenuBasicSettings.Add(form);
        _BackendContext.SaveChanges();
        return CreatedAtAction(nameof(Get_MenuBasicSetting), new { id = form.Id }, form);
    }

    [HttpPut("{id}")]
    public ActionResult<S01MenuBasicSetting> Put_MenuBasicSetting(Guid id, [FromBody] S01MenuBasicSetting form)
    {
        if (id != form.Id) { return BadRequest(); }
        _BackendContext.Entry(form).State = EntityState.Modified;

        try
        {
            _BackendContext.SaveChanges();
        }
        catch (DbException ex)
        {
            if(!_BackendContext.S01MenuBasicSettings.Any(x=>x.Id == id))
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
        var result = _BackendContext.S01MenuBasicSettings.Find(id);
        if (result == null) NotFound();
        _BackendContext.S01MenuBasicSettings.Remove(result);
        _BackendContext.SaveChanges();
        return NoContent();
    }
}
