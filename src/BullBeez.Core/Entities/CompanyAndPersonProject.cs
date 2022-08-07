﻿using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class CompanyAndPersonProject : EntBaseAdvanced
    {
        public int ComponyAndPersonId { get; set; }
        public virtual CompanyAndPerson ComponyAndPerson { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}