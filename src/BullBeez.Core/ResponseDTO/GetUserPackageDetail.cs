using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class GetUserPackageDetail
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string UniqCode { get; set; }
        public string UniqCodeAndroid { get; set; }
        public int Color { get; set; }
        public int ProfileType { get; set; }
    }
}
