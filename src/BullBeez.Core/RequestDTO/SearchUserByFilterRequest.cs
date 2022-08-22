using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class SearchUserByFilterRequest : BaseRequest
    {
        public string Interests { get; set; }
        public int OccupationId { get; set; }
        public string NameOrTitle { get; set; }
        public int ProfileType { get; set; }//Bu firma yada 
        public string CompanyType { get; set; }
        public int CityId { get; set; }
        public string Email { get; set; }
    }
}
