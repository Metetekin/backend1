using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Feedback : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public string Description { get; set; }
    }
}
