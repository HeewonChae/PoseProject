using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Utilities
{
    public static class EnumConverter
    {
        public static bool TryParseEnum<TEnum>(this int enumValue, out TEnum retVal)
        {
            retVal = default;
            bool success = Enum.IsDefined(typeof(TEnum), enumValue);
            if (success)
            {
                retVal = (TEnum)Enum.ToObject(typeof(TEnum), enumValue);
            }
            return success;
        }

        public static bool TryParseEnum<TEnum>(this string enumValue, out TEnum retVal)
        {
            retVal = default;
            bool success = Enum.IsDefined(typeof(TEnum), enumValue);
            if (success)
            {
                retVal = (TEnum)Enum.Parse(typeof(TEnum), enumValue);
            }
            return success;
        }
    }
}