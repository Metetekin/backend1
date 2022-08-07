using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class OccupationResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string HashId { get; set; }
        public int IsSelected { get; set; }
        public int RowStatu { get; set; }
        public string Name { get; set; }
    }
}
