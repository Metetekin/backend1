using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class City : EntBaseAdvanced
    {
        public int PlateNumber { get; set; }
        public string Name { get; set; }
        public virtual ICollection<County> Counties { get; set; }
    }
}
