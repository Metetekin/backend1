using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.RequestDTO
{
    public class CommonDataInsertOrUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CommonType { get; set; }
    }
}