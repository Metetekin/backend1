using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class CompanyAndPerson : EntBaseAdvanced
    {
        public CompanyAndPerson()
        {
            //Project = new Collection<Project>();
            //Skill = new Collection<Skill>();
            //Occupation = new Collection<Occupation>();
            //CompanyAndPersonDetails = new Collection<CompanyAndPersonDetail>();
            //Education = new Collection<Education>();
            //MailApprovedCodes = new Collection<MailApprovedCode>();
            //Tokens = new Collection<Token>();
        }
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
        public int WorkerCount { get; set; }
        public bool IsAdmin { get; set; }
        public virtual ICollection<Address> Address { get; set; }

        public virtual ICollection<CompanyAndPersonProject> CompanyAndPersonProject { get; set; }//kullanıcıya ait proje listesi
        public virtual ICollection<CompanyAndPersonSkills> CompanyAndPersonSkills { get; set; }//Yetenek kişiye ait (Yazılımcı ,tasarımcı, mühendis)
        public virtual ICollection<CompanyAndPersonOccupation> CompanyAndPersonOccupation { get; set; }//Kullanıcıya ait meslek diyebiliriz
        public virtual ICollection<CompanyAndPersonDetail> CompanyAndPersonDetails { get; set; }//Kullanıcıya ait bilgiler
        public virtual ICollection<Education> Education { get; set; }//Kullanıcıya ait eğitim bilgileri
        public virtual ICollection<MailApprovedCode> MailApprovedCodes { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual CompanyType CompanyType { get; set; }
        public virtual CompanyLevel CompanyLevel { get; set; }
        public virtual ICollection<Follows> Follows { get; set; }
        public virtual ICollection<FileData> Files { get; set; }
        public virtual ICollection<CompanyAndPersonInterest> CompanyAndPersonInterests { get; set; }
        public int MailPermission { get; set; }
        public string BannerImage { get; set; }

    }
}
