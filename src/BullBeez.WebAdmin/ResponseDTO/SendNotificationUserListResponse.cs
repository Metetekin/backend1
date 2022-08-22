using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class SendNotificationUserListResponse
    {
        public int Id { get; set; }
        public string NameOrTitle { get; set; }
        public string UserName { get; set; }
        public string FirebaseToken { get; set; }
        public int DeviceIdCount { get; set; }
        public int OccupationId { get; set; }
        public int? BirthDay { get; set; }
    }
}