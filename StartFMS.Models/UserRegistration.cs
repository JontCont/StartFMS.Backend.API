using System;

namespace StartFMS.Models;

public class UserRegistration
{
    public string Account { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
}
