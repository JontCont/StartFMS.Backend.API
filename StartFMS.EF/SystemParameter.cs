using System;
using System.Collections.Generic;

namespace StartFMS.EF;

public partial class SystemParameter
{
    /// <summary>
    /// 識別碼
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 參數1
    /// </summary>
    public string? Value1 { get; set; }

    /// <summary>
    /// 參數2
    /// </summary>
    public string? Value2 { get; set; }

    /// <summary>
    /// 參數3
    /// </summary>
    public string? Value3 { get; set; }

    public string? Description { get; set; }
}
