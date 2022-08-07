
using BullBeez.Core.Enums;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BullBeez.Core.ExpressionBuilder
{
    [DataContract]
    public class ExpressionParameter
    {
        [DataMember]
        public ExpressionOperator Operator { get; set; }

        [DataMember]
        public string PropertyName { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}

