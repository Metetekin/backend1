using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.PaymentApiCore.Model
{
    public class Log
    {
        public string Hata { get; set; }
        public string Zaman { get; set; }
        public string UserDetail { get; set; }
        public string RequestDetail { get; set; }
        public string Guid { get; set; }
    }
}