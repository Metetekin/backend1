using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class ProjectResponse
    {
        public int UserId { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string ProjectName { get; set; }
        public int? MonthCount { get; set; }
        public string Guid { get; set; }
        public string Interests { get; set; }
        public string ProjectRoleName { get; set; }
        public string ProjectDescription { get; set; }
        public List<InterestListResponse> InterestList { get; set; }
    }
}
