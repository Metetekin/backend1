using BullBeez.Core.BaseEntities;
using BullBeez.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Follows : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public int ToUserId { get; set; }
        public EnumFollowType FollowType { get; set; }
        public EnumWorkerFollowType WorkerFollowType { get; set; }
        public string Position { get; set; }
        public bool IsShow { get; set; }

        public void UpdateRowStatuFollows(
            EnumRowStatusType rowStatusType)
        {
            this.RowStatu = rowStatusType;
        }
    }
}
