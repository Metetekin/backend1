using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class ServicePayment
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string NameOrTitle { get; set; }
        public string EmailAddress { get; set; }
        public string GSM { get; set; }
        public string ServiceName { get; set; }
        public decimal Amount { get; set; }
        public string Question { get; set; }
        public string Option { get; set; }
        public string TextCevap { get; set; }
        public string Inserteddate { get; set; }
        public string IsPayment { get; set; }
    }
}