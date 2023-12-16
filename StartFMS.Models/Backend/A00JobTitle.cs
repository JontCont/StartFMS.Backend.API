using System;
using System.Collections.Generic;

namespace StartFMS.Models.Backend
{
    public partial class A00JobTitle
    {
        public A00JobTitle()
        {
            A00Accounts = new HashSet<A00Account>();
        }

        public Guid JobTitleId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<A00Account> A00Accounts { get; set; }
    }
}
