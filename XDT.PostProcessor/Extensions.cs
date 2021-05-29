using System;
using System.Collections.Generic;
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

        public static void AddSortedSection(this List<string> list, IEnumerable<string> values, string header = null)
        {
            var sorted = values.OrderBy(v => v).ToList();
            if (sorted.Count == 0)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(header))
            {
                list.Add(header);
            }
            list.AddRange(sorted);
        }

        public static string ToSortedPipeStringDelimited(this IEnumerable<string> values, bool applyTextWrap = false, string emptyValue = null)
        {
            return values.OrderBy(v => v).ToPipeStringDelimited(applyTextWrap, emptyValue);
        }

        public static string ToPipeStringDelimited(this IEnumerable<string> values, bool applyTextWrap = false, string emptyValue = null)
        {
            if (!string.IsNullOrWhiteSpace(emptyValue))
            {
                values = values.ToArray();
                if (!values.Any())
                {
                    return emptyValue;
                }
            }

            // ReSharper disable PossibleMultipleEnumeration
            return applyTextWrap
                ? "\"" + string.Join("\" | \"", values) + "\""
                : string.Join(" | ", values);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
