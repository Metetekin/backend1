using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BullBeez.Core.ResponseDTO.WebUIResponse.Auth
{

    public class ExternalAuthApple
    {

        public string DeviceId { get; set; }
        public Authorization authorization { get; set; }
        public User user { get; set; }

    }
    public class Name
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
    public class Authorization
    {
        public string code { get; set; }
        public string id_token { get; set; }
        public string state { get; set; }
    }
    public class User
    {
        public string email { get; set; }
        public Name name { get; set; }
    }
}
