using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using StartFMS.Backend.API.Dtos;
using System.Diagnostics;

namespace StartFMS.Backend.API.Filters;

using Serilog;

public class LogExceptionFilter : Attribute, IExceptionFilter
{
    private readonly IWebHostEnvironment _env;
    public LogExceptionFilter(IWebHostEnvironment env)
    {
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        string rootRoot = _env.ContentRootPath + @"\Log\Error\";
        if (!Directory.Exists(rootRoot))
        {
            Directory.CreateDirectory(rootRoot);
        }

        var result = new {
            Path = context.HttpContext.Request.Path,
            Method = context.HttpContext.Request.Method,
            QueryString = context.HttpContext.Request.QueryString,
            Exception = context.Exception.Message,
        };
        string text = $"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] [Error] : {result } \n";
        File.AppendAllText($"{rootRoot}ParterApi_{DateTime.Now.ToString("yyyyMMdd")}.txt", text);

        // 取得 log_tradeID header
        var tradeId = context.HttpContext.Request.Headers["log_tradeID"].FirstOrDefault() ?? "";

        // 使用 Serilog 寫入 Seq，帶入 log_tradeID
        Log.ForContext("log_tradeID", tradeId)
            .Error(context.Exception, "API Exception: {Path} {Method} {QueryString}",
                context.HttpContext.Request.Path,
                context.HttpContext.Request.Method,
                context.HttpContext.Request.QueryString);

        // Handle the exception here and create a custom error response
        context.Result = new ObjectResult(new RetrunJson
        {
            Data = "",
            HttpCode = 400,
            ErrorMessage = context.Exception.Message
        });
        context.ExceptionHandled = true;
    }


}
