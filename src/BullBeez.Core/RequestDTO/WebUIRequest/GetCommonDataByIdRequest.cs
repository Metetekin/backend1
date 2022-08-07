using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO.WebUIRequest
{
    public class GetCommonDataByIdRequest
    {
        public int Id { get; set; }
        public int CommonType { get; set; }
    }
}
