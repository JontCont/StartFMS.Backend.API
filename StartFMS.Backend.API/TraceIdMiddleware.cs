using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Context;

public class TraceIdMiddleware
{
    private const string TraceKey = "X-Log-TraceId";
    private readonly RequestDelegate _next;

    public TraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = context.Request.Headers[TraceKey].FirstOrDefault();
        if (string.IsNullOrEmpty(traceId))
        {
            traceId = Guid.NewGuid().ToString();
            context.Request.Headers[TraceKey] = traceId;
        }
        context.Response.Headers[TraceKey] = traceId;

        using (LogContext.PushProperty(TraceKey, traceId))
        {
            await _next(context);
        }
    }
}