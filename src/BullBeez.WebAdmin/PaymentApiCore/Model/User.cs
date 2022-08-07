using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.PaymentApiCore.Model
{
    public class User
    {
        public int Id { get; set; }
        public int RowStatu { get; set; }
        public string NameOrTitle { get; set; }
        public string UserName { get; set; }
        public string GSM { get; set; }
        public string EmailAddress { get; set; }
    }
}