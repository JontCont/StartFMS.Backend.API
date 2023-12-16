using System;
using System.Collections.Generic;

namespace StartFMS.Models.Backend
{
    public partial class S10SystemConfig
    {
        public string ParName { get; set; } = null!;
        public string ParValue { get; set; } = null!;
        public string ParMemo { get; set; } = null!;
    }
}
