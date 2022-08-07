using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.DTO.MailDTO
{
    public class MailRequest
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public string EmailAddress { get; set; }
        public DateTime SendDate { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
