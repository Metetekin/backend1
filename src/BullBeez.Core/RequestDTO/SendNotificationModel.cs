using BullBeez.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class SendNotificationModel
    {
        public int userId { get; set; }
        public int toUserId { get; set; }
        public int ProfileType { get; set; }
        public EnumNotificationType NotificationType { get; set; }
    }
}
