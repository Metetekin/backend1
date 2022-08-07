using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.PaymentApiCore.Model
{
    public class Service
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public Decimal Amount { get; set; }
        public int DiscountPercentage { get; set; }
        public string ServiceIcon { get; set; }
        public string Description { get; set; }
    }
}