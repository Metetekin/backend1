using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class PackagePayment
    {
        public string NameOrTitle { get; set; }
        public string EmailAddress { get; set; }
        public string GSM { get; set; }
        public string PackageName { get; set; }
        public decimal Amount { get; set; }
        public string Inserteddate { get; set; }
        public string IsPayment { get; set; }

    }
}