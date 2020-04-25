using Newtonsoft.Json;
using System;

namespace PoseSportsPredict.Utilities
{
    public static class SmartJsonSerializer
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
                return default;
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
                return default;
            }

            return result;
        }
    }
}