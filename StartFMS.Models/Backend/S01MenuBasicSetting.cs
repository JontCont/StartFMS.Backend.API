using System;
using System.Collections.Generic;

namespace StartFMS.Models.Backend
{
    public partial class S01MenuBasicSetting
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; } = null!;
        /// <summary>
        /// A: 一般、B: 報表、C: 其他
        /// </summary>
        public string MenuType { get; set; } = null!;
        public string MenuMemo { get; set; } = null!;
    }
}
