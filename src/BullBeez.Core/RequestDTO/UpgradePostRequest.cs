
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class UpgradePostRequest : BaseRequest
    {
        public int PostId { get; set; }
        public bool IsUpgrade { get; set; }
    }
}
