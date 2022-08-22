using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class PackageDatatableModel
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public decimal Amount { get; set; }
        public decimal OldAmount { get; set; }
        public string PackageIcon { get; set; }
        public string Description { get; set; }
        public string UniqCode { get; set; }
        public string UniqCodeAndroid { get; set; }
    }
}