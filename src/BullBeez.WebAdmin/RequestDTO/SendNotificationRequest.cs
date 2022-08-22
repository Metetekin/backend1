using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.RequestDTO
{
    public class SendNotificationRequest
    {
        public int Gender { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public List<int> Occupations { get; set; }
        public int PhoneMark { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public int ProfileType { get; set; }
        public List<int> CompanyType { get; set; }
        public int CompanyAndPersonId { get; set; }
    }
}