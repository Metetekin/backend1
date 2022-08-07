using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class FileData : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public virtual int CompanyAndPersonId { get; set; }
        public virtual string FileName { get; set; }

        public virtual string FileType { get; set; }
        public virtual int? Size { get; set; }

        public virtual byte[] ByteFile { get; set; }
    }
}