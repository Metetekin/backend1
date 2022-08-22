using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class GetMoreCommentsRequest : BaseRequest
    {
        public int PostId { get; set; }
        public int Count { get; set; }
        public int StartIdx { get; set; }
    }
}
