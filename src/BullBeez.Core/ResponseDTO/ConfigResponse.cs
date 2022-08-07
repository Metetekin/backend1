using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class ConfigResponse 
    {
        public string ContractText { get; set; }
        public List<TwitterAccount> TwitterAccount { get; set; }
    }

    public class TwitterAccount 
    {
        public string CKey { get; set; }
        public string CSecret { get; set; }
        public string AToken { get; set; }
        public string TSecret { get; set; }
        public ulong LastTweetId { get; set; }
    }
}
