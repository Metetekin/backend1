using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class ProjectInterest : EntBaseAdvanced
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public int InterestId { get; set; }
        public virtual Interest Interest { get; set; }
    }
}
