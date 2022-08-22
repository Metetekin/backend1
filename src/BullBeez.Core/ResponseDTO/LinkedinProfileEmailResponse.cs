using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class LinkedinProfileEmailResponse
    {
        public List<Element> elements { get; set; }
    }


    public class Element
    {
        [JsonProperty("handle~")]
        public Handle Handle { get; set; }

    }

    public class Handle
    {
        public string emailAddress { get; set; }
    }
}
