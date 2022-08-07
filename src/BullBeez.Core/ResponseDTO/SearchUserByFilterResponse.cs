using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class SearchUserByFilterResponse
    {
        public int UserId { get; set; }
        public string NameOrTitle { get; set; }
        public string BullbeezSentence { get; set; }
        public string Occupation { get; set; }
        public string ProfileImage { get; set; }
        public string Interests { get; set; }
        public int ProfileType { get; set; }
        public int CompanyTypeId { get; set; }
        public string CompanyTypeName { get; set; }

    }
}
