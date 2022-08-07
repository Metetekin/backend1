using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.PaymentApiCore.Model
{
    public class CardDetailModel
    {
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public string NameOrTitle { get; set; }
        public string EmailAddress { get; set; }
        public string GSM { get; set; }
        public string ServiceName { get; set; }
        public Decimal Amount { get; set; }
        public Decimal AmountFirst { get; set; }
        public Decimal KDV { get; set; }
        public Address Address { get; set; }
        public Guid? CreatePaymentId { get; set; }
        public bool ContractConfirmation { get; set; }
    }
}