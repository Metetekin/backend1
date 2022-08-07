using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class PostModel
    {
        public ulong Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserLink { get; set; }
        public string MediaLink { get; set; }
        public string PostLink { get; set; }
    }
}
