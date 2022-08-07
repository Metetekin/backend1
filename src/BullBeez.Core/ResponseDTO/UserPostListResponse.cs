using BullBeez.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class UserPostListResponse
    {
        public string AuthorName { get; set; }
        public string AuthorOccupation { get; set; }
        public string AuthorUserName { get; set; }
        public string PostText { get; set; }
        public string PostMedia { get; set; }
        public string PostTopics { get; set; }
        public DateTime PostCreatedDate { get; set; }
        public int PostLikeCount { get; set; }
        public int PostCommentCount { get; set; }
    }
}
