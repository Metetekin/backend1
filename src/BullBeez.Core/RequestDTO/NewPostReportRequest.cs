using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class NewPostReportRequest : BaseRequest
    {
        public int PostId { get; set; }
        public int ReasonId { get; set; }
        public string ReasonText { get; set; }
    }
}
