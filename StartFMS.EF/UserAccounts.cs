using System;
using System.Collections.Generic;

namespace StartFMS.EF;

public partial class UserAccounts
{
    /// <summary>
    /// 識別碼
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string Account { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 使用者角色ID (UserRole)
    /// </summary>
    public Guid? UserRoleId { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsEnabled { get; set; }
}
