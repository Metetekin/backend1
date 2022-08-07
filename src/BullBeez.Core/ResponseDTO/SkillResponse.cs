using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class SkillResponse
    {
        public int Id { get; set; }
        public string HashId { get; set; }
        public string Name { get; set; }
        public int IsSelected { get; set; }
        public int RowStatu { get; set; }
    }
}
