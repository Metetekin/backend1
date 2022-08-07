using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class SendMailResponse
    {
        public int IsExistsUser { get; set; }
        public string ValidationCode { get; set; }
        public string EmailAddress { get; set; }
        public int? UserId { get; set; }
        public string Token { get; set; }
        public int ProfileType { get; set; }
    }
}
