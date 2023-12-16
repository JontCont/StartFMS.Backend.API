using System;
using System.Collections.Generic;

namespace StartFMS.Models.Backend
{
    public partial class A00Account
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid JobTitleId { get; set; }
        public Guid DivisionId { get; set; }

        public virtual A00Division Division { get; set; } = null!;
        public virtual A00JobTitle JobTitle { get; set; } = null!;
    }
}
