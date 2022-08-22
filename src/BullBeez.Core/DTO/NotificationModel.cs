using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.DTO
{
    public class NotificationModel
    {
        public int UserId { get; set; }
        public string DeviceFirebaseToken { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string TypeString { get; set; }
        public string PostId { get; set; }
    }
}
