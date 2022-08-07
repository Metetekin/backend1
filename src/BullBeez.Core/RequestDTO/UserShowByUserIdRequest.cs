using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class UserShowByUserIdRequest : BaseRequest
    {
        public int RequestUserId { get; set; }
    }
}
