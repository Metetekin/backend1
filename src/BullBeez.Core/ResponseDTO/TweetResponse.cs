using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class TweetResponse
    {
        public ulong id { get; set; }
        public string text { get; set; }
        public TweetUser user { get; set; }
        public Entities extended_entities { get; set; }

    }

    public class TweetUser
    {
        public string name { get; set; }
        public string profile_image_url_https { get; set; }
    }
    public class Entities
    {
        public List<Media> media { get; set; }
    }
    public class Media
    {
        public string media_url_https { get; set; }
    }
}
