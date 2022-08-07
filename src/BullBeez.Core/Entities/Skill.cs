using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Skill : EntBaseAdvanced
    {
        public string Name { get; set; }
        public virtual ICollection<CompanyAndPersonSkills> CompanyAndPersonSkills { get; set; }
    }
}
