using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using StartFMS.Backend.API.Dtos;

namespace StartFMS.Backend.API.Filters;

public class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    public string Roles = "";
    public AuthorizationFilter()
    {

    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool tokenFlag = context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues outValue);

        var ignore = (from a in context.ActionDescriptor.EndpointMetadata
                      where a.GetType() == typeof(AllowAnonymousAttribute)
                      select a).FirstOrDefault();

        if (ignore == null)
        {
            if (tokenFlag)
            {
                if (outValue != "123")
                {
                    context.Result = new JsonResult(new RetrunJson()
                    {
                        Data = "test1",
                        HttpCode = 401,
                        ErrorMessage = "沒有登入"
                    });
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new JsonResult(new RetrunJson()
                {
                    Data = "test2",
                    HttpCode = 401,
                    ErrorMessage = "沒有登入"
                });
            }
        }//if (ignore == null)

    }// void OnAuthorization()
}
