using System;
using System.Collections.Generic;

namespace StartFMS.Models.Backend
{
    public partial class A00Role
    {
        public Guid RoleId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Name { get; set; } = null!;
    }
}
