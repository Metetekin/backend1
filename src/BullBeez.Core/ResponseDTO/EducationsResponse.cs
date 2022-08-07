using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    class EducationsResponse
    {
        public int UserId { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string SchoolName { get; set; }
    }
}
