using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.RequestDTO
{
    public class NewPostRequest
    {
        public int? UserId { get; set; }
        public string PostText { get; set; }
        public string PostMedia { get; set; }
        public string PostTopicsStr { get; set; }
        public bool IsSponsoredPost { get; set; }
    }
}