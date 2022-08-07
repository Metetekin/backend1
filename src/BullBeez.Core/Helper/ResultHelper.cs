using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BullBeez.Core.Helper
{
    public static class ResultHelper
    {
        public static T DeserializeObject<T>(this string objectData)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(objectData);

        }

        public static string SerializeObject(this object objectData)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(objectData);

        }
    }
}
