using BullBeez.Core.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class PostReport : EntBaseAdvanced
    {
        public PostReport()
        {
        }
        public int PostId { get; set; }
        public int ReasonId { get; set; }
        public string ReasonText { get; set; }
    }
}
