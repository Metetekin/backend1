using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class PostDatatableModel
    {
        public int Id { get; set; }
        public string PostText { get; set; }
        public string PostMedia { get; set; }
        public string NameOrTitle { get; set; }
        public int CountData { get; set; }
    }
}