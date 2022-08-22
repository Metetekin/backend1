using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class GetPostCommentsByPostIdRequest : BaseRequest
    {
        public int PostId { get; set; }
        public string Count { get; set; }
    }
}
