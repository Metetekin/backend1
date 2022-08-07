using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.PaymentApiCore.Model
{
    public class PackageModel
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public Decimal Amount { get; set; }
        public int DiscountPercentage { get; set; }
        public string PackageIcon { get; set; }
        public string Description { get; set; }
    }
}