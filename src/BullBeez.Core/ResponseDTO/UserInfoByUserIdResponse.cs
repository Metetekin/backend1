using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class UserInfoByUserIdResponse
    {
        public int UserId { get; set; }
        public string NameOrTitle { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string GSM { get; set; }
        public string Status { get; set; }
        public string BullbeezSentence { get; set; }
        public string Biography { get; set; }
        public int OccupationId { get; set; }
        public string Occupation { get; set; }
        public string EstablishDate { get; set; }
        public List<InterestListResponse> InterestList { get; set; }
        public List<SkillResponse> SkillList { get; set; }
        public int IsShowSkill { get; set; }
        public string ProfileImage { get; set; }
        public List<EducationResponse> EducationList { get; set; }
        public List<ProjectResponse> ProjectList { get; set; } = new List<ProjectResponse>();
        public int FollowCount { get; set; }
        public int ProfileType { get; set; }
        public int? CompanyTypeId { get; set; }
        public string CompanyTypeName { get; set; }
        public int? CompanyLevelId { get; set; }
        public string CompanyLevelName { get; set; }
        public bool IsVenture { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountyId { get; set; }
        public string CountyName { get; set; }
        public string Address { get; set; }
        public int? IsFollow { get; set; }
        public int? IsWorker { get; set; }
        public string FileName { get; set; }
        public string Base64String { get; set; }
        public string CompanyDescription { get; set; }
        public int WorkerCount { get; set; }
        public List<GetFollowUserByUserIdResponse> WorkerList { get; set; }
        public int MailPermission { get; set; }
        public string BannerImage { get; set; }
}
}
