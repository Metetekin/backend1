using BullBeez.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class FollowUserByUserIdRequest : BaseRequest
    {
        public int ToUserId { get; set; }
        public EnumFollowType FollowType { get; set; }
        public EnumWorkerFollowType WorkerFollowType { get; set; }
        public string Position { get; set; }
    }
}
