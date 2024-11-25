using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StartFMS.Backend.API.Dtos;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StartFMS.Backend.API.Filters
{
    public class ApiResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
           
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new JsonResult(new RetrunJson
                {
                    Data = null,
                    HttpCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = "資料驗證失敗"
                });
            }

            if (context.Result is ObjectResult objectResult)
            {
                var data = objectResult.Value ?? null;
                var httpCode = (int)(HttpStatusCode)(objectResult.StatusCode ?? 200);
                var errorMessage = "";

                if(data is RetrunJson returnJson)
                {
                    context.Result = new JsonResult(returnJson);
                }
                else
                {
                    context.Result = new JsonResult(new RetrunJson
                    {
                        Data = data,
                        HttpCode = httpCode,
                        ErrorMessage = errorMessage
                    });
                }
            }
        }
    }
}
