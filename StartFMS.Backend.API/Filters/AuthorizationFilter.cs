using Microsoft.AspNetCore.Mvc.Filters;

namespace StartFMS.Backend.API.Filters;

public class AuthorizationFilter : Attribute, IAuthorizationFilter
{
    public string Roles = "";
    public AuthorizationFilter()
    {

    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //TodoContext _todoContext = (TodoContext)context.HttpContext.RequestServices.GetService(typeof(TodoContext));

        //var role = (from a in _todoContext.Roles
        //            where a.Name == Roles
        //            select a).FirstOrDefault();

        //if (role == null)
        //{
        //    context.Result = new JsonResult(new RetrunJson()
        //    {
        //        Data = Roles,
        //        HttpCode = 401,
        //        ErrorMessage = "沒有登入"
        //    });
        //}

    }
}
