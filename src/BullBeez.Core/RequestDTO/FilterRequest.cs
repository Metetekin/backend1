using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class FilterRequest : BaseRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int OrderBy { get; set; }
    }
}
