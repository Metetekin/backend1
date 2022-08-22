using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO.WebUIRequest
{
    public class PopUpInsertRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int PageNumber { get; set; }
    }
}
