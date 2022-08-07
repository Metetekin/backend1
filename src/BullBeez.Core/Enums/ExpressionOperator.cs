﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BullBeez.Core.Enums
{
    [DataContract]
    public enum ExpressionOperator
    {
        [EnumMember]
        Contains = 6,
        [EnumMember]
        EndsWith = 8,
        [EnumMember]
        Equals = 0,
        [EnumMember]
        GreaterThan = 2,
        [EnumMember]
        GreaterThanOrEqual = 4,
        [EnumMember]
        LessThan = 3,
        [EnumMember]
        LessThanOrEqual = 5,
        [EnumMember]
        NotEquals = 1,
        [EnumMember]
        StartsWith = 7
    }
}
