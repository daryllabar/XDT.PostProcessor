using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XDT.PostProcessor.Test
{
    public static class StringAssertExtensions
    {
        public static void ShouldEqualWithDiff(this IEnumerable<string> actualValue, string expectedValue)
        {
            ShouldEqualWithDiff(string.Join(Environment.NewLine, actualValue), expectedValue, DiffStyle.Full, Console.Out);
        }

        public static void ShouldEqualWithDiff(this string actualValue, string expectedValue)
        {
            ShouldEqualWithDiff(actualValue, expectedValue, DiffStyle.Full, Console.Out);
        }

        public static void ShouldEqualWithDiff(this string actualValue, string expectedValue, DiffStyle diffStyle)
        {
            ShouldEqualWithDiff(actualValue, expectedValue, diffStyle, Console.Out);
        }

        public static void ShouldEqualWithDiff(this string actualValue, string expectedValue, DiffStyle diffStyle, TextWriter output)
        {
            if (actualValue == null || expectedValue == null)
            {
                Assert.AreEqual(expectedValue, actualValue);
                return;
            }

            if (actualValue.Equals(expectedValue, StringComparison.Ordinal)) return;

            var lines = new List<string>();
            var firstDifference = -1;
            var maxLen = Math.Max(actualValue.Length, expectedValue.Length);
            var minLen = Math.Min(actualValue.Length, expectedValue.Length);
            for (var i = 0; i < maxLen; i++)
            {
                if (diffStyle != DiffStyle.Minimal || i >= minLen || actualValue[i] != expectedValue[i])
                {
                    var isDifferent = i >= minLen || actualValue[i] != expectedValue[i];
                    if (isDifferent && firstDifference == -1)
                    {
                        firstDifference = i;
                    }
                    lines.Add(string.Format("{0} {1,-3} {2,-4} {3,-3}  {4,-4} {5,-3}",
                        isDifferent ? "*" : " ", // put a mark beside a differing row
                        i, // the index
                        i < expectedValue.Length ? ((int)expectedValue[i]).ToString() : "", // character decimal value
                        i < expectedValue.Length ? expectedValue[i].ToSafeString() : "", // character safe string
                        i < actualValue.Length ? ((int)actualValue[i]).ToString() : "", // character decimal value
                        i < actualValue.Length ? actualValue[i].ToSafeString() : "" // character safe string
                    ));
                }
            }

            if (firstDifference >= 0)
            {
                var start = firstDifference < 20
                    ? 0
                    : firstDifference - 20;
                var end = start + 20 > lines.Count - 1
                    ? lines.Count - 1
                    : start + 20;
                output.WriteLine("  Idx Expected  Actual");
                output.WriteLine("-------------------------");
                foreach (var line in lines.Skip(start).Take(end - start))
                {
                    output.WriteLine(line);
                }

                if (expectedValue.Length >= start
                    && actualValue.Length >= start)
                {
                    Assert.AreEqual(Environment.NewLine + expectedValue.Substring(start), Environment.NewLine + actualValue.Substring(start));
                }
            }

            Assert.AreEqual(expectedValue, actualValue);
        }

        private static string ToSafeString(this char c)
        {
            if (char.IsControl(c) || char.IsWhiteSpace(c))
            {
                switch (c)
                {
                    case '\r':
                        return @"\r";
                    case '\n':
                        return @"\n";
                    case '\t':
                        return @"\t";
                    case '\a':
                        return @"\a";
                    case '\v':
                        return @"\v";
                    case '\f':
                        return @"\f";
                    default:
                        return string.Format("\\u{0:X};", (int)c);
                }
            }
            return c.ToString(CultureInfo.InvariantCulture);
        }
    }

    public enum DiffStyle
    {
        Full,
        Minimal
    }
}
