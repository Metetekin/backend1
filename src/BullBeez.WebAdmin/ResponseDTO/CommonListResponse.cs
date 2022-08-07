using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class CommonListResponse 
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int IsSelected { get; set; }
        public int RowStatu { get; set; }
        public string Name { get; set; }
    }
}