using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class GlobalListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVenture { get; set; }
        public int RowStatu { get; set; }
    }
}