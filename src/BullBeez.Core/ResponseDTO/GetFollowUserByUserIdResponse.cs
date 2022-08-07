using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class GetFollowUserByUserIdResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string BullbeezSentence { get; set; }
        public int OccupationId { get; set; }
        public string Occupation { get; set; }
        public string ProfileImage { get; set; }
        public int ProfileType { get; set; }
        public string Message { get; set; }
        public string Position { get; set; }
        public int? CompanyTypeId { get; set; }
        public string CompanyTypeName { get; set; }
        public int? CompanyLevelId { get; set; }
        public string CompanyLevelName { get; set; }
        public bool IsVenture { get; set; }
        public bool IsShow { get; set; }
    }
}
