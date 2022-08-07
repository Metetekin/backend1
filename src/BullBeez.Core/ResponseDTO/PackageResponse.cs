using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class PackageResponse
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public decimal Amount { get; set; }
        public decimal OldAmount { get; set; }
        public string PackageIcon { get; set; }
        public string Description { get; set; }
    }
}
