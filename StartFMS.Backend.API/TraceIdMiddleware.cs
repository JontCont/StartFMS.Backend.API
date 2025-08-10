using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Context;
using Serilog;

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
            traceId = Guid.NewGuid().ToString("N"); // 無 -
            context.Request.Headers[TraceKey] = traceId;
        }
        context.Response.Headers[TraceKey] = traceId;

        using (LogContext.PushProperty(TraceKey, traceId))
        {
            // 將所有 request headers 寫入 log
            var headers = string.Join("; ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"));
            Log.Information("Request Headers: {Headers}", headers);

            await _next(context);
        }
    }
}