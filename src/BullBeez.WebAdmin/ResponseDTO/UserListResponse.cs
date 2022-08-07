using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class UserListResponse
    {
        public int Id { get; set; }
        public int RowStatu { get; set; }
        public string NameOrTitle { get; set; }
        public string UserName { get; set; }
        public string GSM { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string BullbeezSentence { get; set; }
        public object ConnectCount { get; set; }
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