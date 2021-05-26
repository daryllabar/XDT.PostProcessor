using System;
using System.Linq;

namespace XDT.PostProcessor
{
    public static class Extensions
    {
        public static string Capitalize(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            if (value.Length == 1)
            {
                value = value.ToUpper();
            }
            else if (value.IndexOf("_", StringComparison.Ordinal) > -1)
            {
                var parts = value.Split(new[] { '_' }, StringSplitOptions.None);
                value = string.Join("_", parts.Select(Capitalize));
            }
            else
            {
                value = char.ToUpper(value[0]) + value.Substring(1);
            }

            return value;
        }
	}
}
