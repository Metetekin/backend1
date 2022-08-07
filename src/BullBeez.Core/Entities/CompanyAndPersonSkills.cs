using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class CompanyAndPersonSkills : EntBaseAdvanced
    {
        public int ComponyAndPersonId { get; set; }
        public virtual CompanyAndPerson ComponyAndPerson { get; set; }
        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
