using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StartFMS.Backend.Line.WebAPI.Controllers;

[ApiController]
[Route("/Chat/")]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;

    public ChatController(ILogger<ChatController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<string> Post(string message)
    {
        //await Chat.ResponseMessageAsync(message);
        return JsonConvert.SerializeObject(new {
            Success = true,
            Message = "",
        });
    }


}
