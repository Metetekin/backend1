using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class NewUserContract : BaseRequest
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string GSM { get; set; }
        public string Status { get; set; }
        public string BullbeezSentence { get; set; }
        public string Biography { get; set; }
        public int OccupationId { get; set; }
        public string ProfileImage { get; set; }
        public int ProfileType { get; set; }
        public string Skills { get; set; }
        public string Interests { get; set; }
        public int CountyId { get; set; }
        public string Address { get; set; }
        public int CompanyType { get; set; }
        public int? CompanyLevel { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Gender { get; set; }
        public string FirebaseToken { get; set; }
    }
}
