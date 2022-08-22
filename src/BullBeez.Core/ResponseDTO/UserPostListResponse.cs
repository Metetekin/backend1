using BullBeez.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class UserPostListResponse
    {
        public string PostId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorOccupation { get; set; }
        public string AuthorUserName { get; set; }
        public int AuthorUserId { get; set; }
        public int AuthorProfileType { get; set; }
        public string AuthorLogo { get; set; }
        public string AuthorPackageIcon { get; set; }
        public string PostText { get; set; }
        public string PostMedia { get; set; }
        public string PostTopics { get; set; }
        public int PostCreatedDate { get; set; }
        public int PostLikeCount { get; set; }
        public int PostCommentCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsUpgradedToBoard { get; set; }
        public string SponsoredTitle { get; set; }
    }
}
