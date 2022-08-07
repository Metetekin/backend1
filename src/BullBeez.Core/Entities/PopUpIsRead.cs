using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class PopUpIsRead : EntBaseAdvanced
    {
        public int PopUpId { get; set; }
        public int UserId { get; set; }
    }
}
