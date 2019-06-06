using System;
using System.Globalization;

namespace TeklaHouse
{
    public static class ExtendedString
    {
        public static double ToDouble(this string s)
        {
            double value;
            var style = NumberStyles.AllowDecimalPoint;
            var culture = CultureInfo.InvariantCulture;
            Double.TryParse(s, style, culture, out value);
            return value;
        }
    }
}
