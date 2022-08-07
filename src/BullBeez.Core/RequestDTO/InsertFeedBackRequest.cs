using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class InsertFeedBackRequest : BaseRequest
    {
        public string Description { get; set; }
    }
}
