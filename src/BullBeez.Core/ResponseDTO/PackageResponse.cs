using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class PackageResponse
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public decimal Amount { get; set; }
        public decimal OldAmount { get; set; }
        public string PackageIcon { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string UniqCode { get; set; }
        public string UniqCodeAndroid { get; set; }
        public int Color { get; set; }
        public int ProfileType { get; set; }
        public string Note { get; set; }
        public string SubscriptionNote { get; set; }
        public string SubscriptionEula { get; set; }
    }
}
