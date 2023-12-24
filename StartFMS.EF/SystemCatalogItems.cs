using System;
using System.Collections.Generic;

namespace StartFMS.EF;

public partial class SystemCatalogItems
{
    /// <summary>
    /// 目錄識別碼
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 目錄名稱
    /// </summary>
    public string MenuName { get; set; } = null!;

    /// <summary>
    /// 備註
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 顯示順序 (透過Id抓取，判斷在第幾層位置)
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// 網址..
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Icon
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 父層ID (目前設為 Id)
    /// </summary>
    public int? ParentId { get; set; }
}
