using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Occupation : EntBaseAdvanced
    {
        public string Name { get; set; }
        public int IsShowSkill { get; set; }
        public virtual ICollection<CompanyAndPersonOccupation> ComponyAndPersonOccupations { get; set; }
    }
}
