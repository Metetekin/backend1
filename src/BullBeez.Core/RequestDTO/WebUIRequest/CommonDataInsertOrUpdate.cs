using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO.WebUIRequest
{
    public class CommonDataInsertOrUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CommonType { get; set; }
        public int RowStatu { get; set; }
    }
}
