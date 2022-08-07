using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class UpdateUserInfoByUserIdRequest : BaseRequest
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string BullbeezSentence { get; set; }
        public string Status { get; set; }
        public string Interests { get; set; }
        public string IsShowCompanies { get; set; }
        public string Information { get; set; }
        public string NewPassword { get; set; }
        public string Skills { get; set; }
        public string GSM { get; set; }
        public int OccupationId { get; set; }
        public string EstablishDate { get; set; }
        public int CompanyTypeId { get; set; }
        public int CompanyLevelId { get; set; }
        public string CompanyDescription { get; set; }
        public int CityId { get; set; }
        public int CountyId { get; set; }
        public string Address { get; set; }
        public int WorkerCount { get; set; }
    }
}
