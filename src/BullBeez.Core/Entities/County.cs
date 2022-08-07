using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class County : EntBaseAdvanced
    {
        public string Name { get; set; }
        public virtual City City { get; set; }
    }
}
