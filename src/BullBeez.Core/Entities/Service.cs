using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Service : EntBaseAdvanced
    {
        public string ServiceName { get; set; }
        public Decimal Amount { get; set; }
        public int DiscountPercentage { get; set; }
        public string ServiceIcon { get; set; }
        public string Description { get; set; }
        public ICollection<ServiceQuestion> ServiceQuestions { get; set; }
    }
}
