using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class GetUserPostByIdRequest : BaseRequest
    {
        public int PostId { get; set; }
    }
}
