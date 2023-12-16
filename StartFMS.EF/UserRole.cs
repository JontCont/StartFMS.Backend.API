using System;
using System.Collections.Generic;

namespace StartFMS.EF;

public partial class UserRole
{
    /// <summary>
    /// 識別碼
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 備註
    /// </summary>
    public string? Description { get; set; }

    public bool IsEnabled { get; set; }
}
