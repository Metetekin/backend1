using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Project : EntBaseAdvanced
    {
        public virtual ICollection<CompanyAndPersonProject> CompanyAndPersonProject { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProjectName { get; set; }
        public int? MonthCount { get; set; }
        public string Guid { get; set; }
        public string ProjectRoleName { get; set; }
        public string ProjectDescription { get; set; }
        public virtual ICollection<ProjectInterest> ProjectInterest { get; set; }
    }
}
