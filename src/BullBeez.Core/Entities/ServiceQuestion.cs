using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class ServiceQuestion : EntBaseAdvanced
    {
        public virtual Service Service { get; set; }
        public virtual Questions Question { get; set; }
    }
}
