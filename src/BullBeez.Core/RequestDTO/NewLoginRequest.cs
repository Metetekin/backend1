using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class NewLoginRequest : BaseRequest
    {
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public int ValidationCode { get; set; }
        public string FirebaseToken { get; set; }
    }
}
