using BullBeez.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO.WebUIResponse
{
    public class UserListResponse
    {
        public int Id { get; set; }
        public EnumRowStatusType? RowStatu { get; set; }
        public string NameOrTitle { get; set; }
        public string UserName { get; set; }
        public string GSM { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string BullbeezSentence { get; set; }
        public int? ConnectCount { get; set; }
        public string Status { get; set; }
        public string Biography { get; set; }
        public string ProfileImage { get; set; }
        public string CompanyDescription { get; set; }
        public int ProfileType { get; set; }
        public DateTime? EstablishDate { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Gender { get; set; }
        public int CountData { get; set; }
    }
}
