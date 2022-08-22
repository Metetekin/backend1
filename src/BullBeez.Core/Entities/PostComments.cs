using BullBeez.Core.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class PostComments : EntBaseAdvanced
    {
        public PostComments()
        {
        }
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public virtual UserPosts UserPosts { get; set; }
        public string Text { get; set; }
        public int LikeCount { get; set; }
        public string UserIdWhoLike { get; set; }
    }
}
