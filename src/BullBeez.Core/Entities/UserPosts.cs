using System;
using System.Collections.Generic;
using System.Text;
using BullBeez.Core.BaseEntities;

namespace BullBeez.Core.Entities
{
   public class UserPosts : EntBaseAdvanced
    {
        public UserPosts()
        {
        }
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public string PostText { get; set; }
        public string PostMedia { get; set; }
        public string PostTopics { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public string UserIdWhoLike { get; set; }
        public bool IsSponsoredPost { get; set; }
        public bool IsUpgradedToBoard { get; set; }
        public string SponsoredTitle { get; set; }
    }
}
