using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Education : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SchoolName { get; set; }
    }
}
