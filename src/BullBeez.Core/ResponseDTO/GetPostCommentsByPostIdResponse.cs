using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class PostCommentsByPostIdResponse
    {
        public string CommentId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorLogo { get; set; }
        public string AuthorUserName { get; set; }
        public int AuthorProfileType { get; set; }
        public int AuthorUserId { get; set; }
        public string AuthorOccupation { get; set; }
        public string CommentText { get; set; }
        public int CommentCreatedDate { get; set; }
        public int CommentLikeCount { get; set; }
        public bool IsLiked { get; set; }
    }
}
