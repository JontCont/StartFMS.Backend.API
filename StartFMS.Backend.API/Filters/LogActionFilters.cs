using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace StartFMS.Backend.API.Filters
{
    public class LogActionFilters: IActionFilter
    {
        private readonly IWebHostEnvironment _env;
        public LogActionFilters(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string rootRoot = _env.ContentRootPath + @"\Log\Action\";

            if (!Directory.Exists(rootRoot))
            {
                Directory.CreateDirectory(rootRoot);
            }

            var result = new
            {
                Path = context.HttpContext.Request.Path,
                Method = context.HttpContext.Request.Method,
                QueryString = context.HttpContext.Request.QueryString,
                Employeeid = context.HttpContext.User.FindFirst("EmployeeId")
            };

            string text = $"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] [Info] : {result} \n";
            File.AppendAllText($"{rootRoot}ParterApi_{DateTime.Now.ToString("yyyyMMdd")}.txt", text);

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string rootRoot = _env.ContentRootPath + @"\Log\Action\";

            if (!Directory.Exists(rootRoot))
            {
                Directory.CreateDirectory(rootRoot);
            }

            var result = new
            {
                Path = context.HttpContext.Request.Path,
                Method = context.HttpContext.Request.Method,
                QueryString = context.HttpContext.Request.QueryString,
                Employeeid = context.HttpContext.User.FindFirst("EmployeeId")
            };

            string text = $"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}] [Info] : {result} \n";
            File.AppendAllText($"{rootRoot}ParterApi_{DateTime.Now.ToString("yyyyMMdd")}.txt", text);
        }
    }
}
