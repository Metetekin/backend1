
using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Posts : EntBaseAdvanced
    {
        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserLink { get; set; }
        public string MediaLink { get; set; }
        public string PostLink { get; set; }
    }
}
