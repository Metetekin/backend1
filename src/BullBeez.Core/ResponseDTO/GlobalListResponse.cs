using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class GlobalListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVenture { get; set; }
        public int RowStatu { get; set; }
    }
}
