using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class NewPostRequest : BaseRequest
    {
        public string PostText { get; set; }
        public string PostMedia { get; set; }
        public string PostTopicsStr { get; set; }
        public bool IsSponsoredPost { get; set; }
        public bool IsUpgradedToBoard { get; set; }
        public string SponsoredTitle { get; set; }
    }
}
