using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class PostDatatableModel
    {
        public int Id { get; set; }
        public int RowStatu { get; set; }
        public string PostText { get; set; }
        public string PostMedia { get; set; }
        public string NameOrTitle { get; set; }
        public string PostTopics { get; set; }
        public bool IsSponsoredPost { get; set; }
        public int CountData { get; set; }
        public int CompanyAndPersonId { get; set; }
    }
}
