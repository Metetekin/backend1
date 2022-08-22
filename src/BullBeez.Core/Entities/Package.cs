using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Package : EntBaseAdvanced
    {
        public string PackageName { get; set; }
        public Decimal Amount { get; set; }
        public int DiscountPercentage { get; set; }
        public string PackageIcon { get; set; }
        public string Description { get; set; }
        public string UniqCode { get; set; }
        public string UniqCodeAndroid { get; set; }
        public int Color { get; set; }
        public int ProfileType { get; set; }
        public string Note { get; set; }
        public string SubscriptionNote { get; set; }
        public string SubscriptionEula { get; set; }
    }
}
