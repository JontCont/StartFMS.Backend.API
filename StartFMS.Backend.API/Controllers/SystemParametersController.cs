using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Backend.API.Dtos;
using StartFMS.Backend.Extensions;
using StartFMS.EF;
using StartFMS.Entity;
using System.Net;

namespace StartFMS.Backend.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/sys/parameter")]
public class SystemParametersController : Controller
{
    private readonly ILogger<SystemParametersController> _logger;
    private readonly StartFmsBackendContext _context;
    private readonly JwtHelpers _jwtHelpers;
    private readonly ISystemManagement _systems;

    public SystemParametersController(
        ILogger<SystemParametersController> logger,
        ISystemManagement systems)
    {
        _logger = logger;
        _systems = systems;
    }

    [HttpGet("list")]
    public IEnumerable<SystemParameter>? GetSystemParameters()
    {
        return _systems.GetSystemParameters();
    }

    [HttpGet]
    public IEnumerable<SystemParameter>? GetSystemParameters(string name)
    {
        return _systems.GetSystemParameters(name);
    }

    [HttpPost]
    public IActionResult AddSystemParameter(SystemParameter? value)
    {
        if (value == null)
        {
            return BadRequest(new RetrunJson
            {
                Data = null,
                HttpCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = "參數錯誤"
            });
        }

        var res = _systems.SaveSystemParameters(value);
        return Ok(new RetrunJson
        {
            Data = res,
            HttpCode = (int)HttpStatusCode.OK,
            ErrorMessage = null
        });
    }

    [HttpPut]
    public IActionResult UpdateSystemParameter(SystemParameter? value)
    {
        if (value == null)
        {
            return BadRequest(new RetrunJson
            {
                Data = null,
                HttpCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = "參數錯誤"
            });
        }

        var res = _systems.SaveSystemParameters(value);
        return Ok(new RetrunJson
        {
            Data = res,
            HttpCode = (int)HttpStatusCode.OK,
            ErrorMessage = null
        });
    }

    [HttpDelete]
    public IActionResult DeleteSystemParameter(SystemParameter? value)
    {
        if (value == null)
        {
            return BadRequest(new RetrunJson
            {
                Data = null,
                HttpCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = "參數錯誤"
            });
        }

        _systems.DeleteSystemParameters(value.Id);
        return Ok();
    }
}
