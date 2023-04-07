using System;
using System.Collections.Generic;

namespace StartFMS.Models.Backend
{
    public partial class S01MenuBasicSetting
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; } = null!;
        public string? Description { get; set; }
        /// <summary>
        /// 顯示順序 (透過Id抓取，判斷在第幾層位置)
        /// </summary>
        public int DisplayOrder { get; set; }
        public string? Url { get; set; }
        /// <summary>
        /// 畫面Icon
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 父層ID (目前設為 Id)
        /// </summary>
        public Guid? ParentId { get; set; }


        public ICollection<S01MenuBasicSetting>? Children { get; set; }
        public S01MenuBasicSetting? Parent { get; set; }
    }
}
