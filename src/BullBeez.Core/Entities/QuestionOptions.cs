using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class QuestionOptions : EntBaseAdvanced
    {
        public virtual Questions Question { get; set; }
        public string Option { get; set; }
    }
}
