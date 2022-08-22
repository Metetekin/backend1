using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class LikeByIdRequest : BaseRequest
    {
        public int Id { get; set; }
        public bool Like { get; set; }
    }
}
