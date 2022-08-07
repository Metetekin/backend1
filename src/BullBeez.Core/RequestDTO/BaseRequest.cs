using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class BaseRequest
    {
        public int? UserId { get; set; }
        public string DeviceId { get; set; }
        public string TokenId { get; set; }
        public string EmailAddress { get; set; }
    }
}
