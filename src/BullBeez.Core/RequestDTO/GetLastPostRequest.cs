using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class GetLastPostByUserIdRequest : BaseRequest
    {
        public int PostOwner { get; set; }
    }
}
