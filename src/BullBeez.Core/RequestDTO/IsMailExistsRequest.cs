using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class IsMailExistsRequest : BaseRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }
}
