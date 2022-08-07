using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class GetUserPostsRequest : BaseRequest
    {
        public int RequestCount { get; set; }
        public string FilterTopicsStr { get; set; }
    }
}
