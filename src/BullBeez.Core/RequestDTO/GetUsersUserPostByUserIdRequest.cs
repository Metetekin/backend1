using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class GetUsersUserPostByUserIdRequest : BaseRequest
    {
        public int StartIndex { get; set; }
        public int RequestCount { get; set; }
        public string ShowingUserName { get; set; }
    }
}
