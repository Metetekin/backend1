using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class CompanyAndPersonInterest : EntBaseAdvanced
    {
        public int ComponyAndPersonId { get; set; }
        public virtual CompanyAndPerson ComponyAndPerson { get; set; }
        public int InterestId { get; set; }
        public virtual Interest Interest { get; set; }
    }
}
