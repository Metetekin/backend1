using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class UserNotificationResponse
    {
        public int UserId { get; set; }
        public int ToUserId { get; set; }
        public string Message { get; set; }
        public string ProfileImage { get; set; }
        public int IsShow { get; set; }
        public int ProfileType { get; set; }
    }
}
