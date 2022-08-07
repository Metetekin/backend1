using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Questions : EntBaseAdvanced
    {
        public virtual ICollection<ServiceQuestion> ServiceQuestions { get; set; }
        public string Question { get; set; }
        public int QuestionType { get; set; }
        public virtual ICollection<QuestionOptions> QuestionOptions { get; set; }
    }
}
