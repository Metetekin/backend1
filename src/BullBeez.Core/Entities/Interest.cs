using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Interest : EntBaseAdvanced
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public virtual ICollection<CompanyAndPersonInterest> CompanyAndPersonInterests { get; set; }
        public virtual ICollection<ProjectInterest> ProjectInterest { get; set; }
    }
}
