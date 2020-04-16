using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReferenceTest.NetCore
{
    public static class SmartJsonConverter
    {
        public static TOut JsonDeserialize<TOut>(this string jsonString)
        {
            TOut result = default;

            if (string.IsNullOrEmpty(jsonString))
                return result;

            try
            {
                result = JsonConvert.DeserializeObject<TOut>(jsonString);
            }
            catch (Exception)
            {
                return result;
            }

            return result;
        }

        public static string JsonSerialize(this object @object)
        {
            string result = string.Empty;

            if (@object == null)
                return result;

            try
            {
                result = JsonConvert.SerializeObject(@object);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return result;
        }
    }
}