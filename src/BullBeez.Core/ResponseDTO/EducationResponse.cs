using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class EducationResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string SchoolName { get; set; }
        public string Guid { get; set; }
    }
}
