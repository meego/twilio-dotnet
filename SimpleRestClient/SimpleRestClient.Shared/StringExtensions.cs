﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Simple
{
    /// <summary>
    /// Method Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Uses Uri.EscapeDataString() based on recommendations on MSDN
        /// http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx
        /// </summary>
        public static string UrlEncode(this string input)
        {
            const int maxLength = 32766;
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.Length <= maxLength)
                return Uri.EscapeDataString(input);

            StringBuilder sb = new StringBuilder(input.Length * 2);
            int index = 0;
            while (index < input.Length)
            {
                int length = Math.Min(input.Length - index, maxLength);
                string subString = input.Substring(index, length);
                sb.Append(Uri.EscapeDataString(subString));
                index += subString.Length;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Check that a string is not null or empty
        /// </summary>
        /// <param name="input">String to check</param>
        /// <returns>bool</returns>
        public static bool HasValue(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Read a stream into a byte array
        /// </summary>
        /// <param name="input">Stream to read</param>
        /// <returns>byte[]</returns>
        public static byte[] ReadAsBytes(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts a byte array to a string, using its byte order mark to convert it to the right encoding.
        /// http://www.shrinkrays.net/code-snippets/csharp/an-extension-method-for-converting-a-byte-array-to-a-string.aspx
        /// </summary>
        /// <param name="buffer">An array of bytes to convert</param>
        /// <returns>The byte as a string.</returns>
        public static string AsString(this byte[] buffer)
        {
            if (buffer == null) return "";

            // Ansi as default
            Encoding encoding = Encoding.UTF8;

            return encoding.GetString(buffer,0, buffer.Length);
        }

        /// <summary>
        /// Return possible variants of a name for name matching.
        /// </summary>
        /// <param name="name">String to convert</param>
        /// <param name="culture">The culture to use for conversion</param>
        /// <returns>IEnumerable&lt;string&gt;</returns>
        public static IEnumerable<string> GetNameVariants(this string name, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(name))
                yield break;

            yield return name;

            // try camel cased name
            yield return name.ToCamelCase(culture);

            // try lower cased name
            yield return name.ToLower();

            // try name with underscores
            yield return name.AddUnderscores();

            // try name with underscores with lower case
            yield return name.AddUnderscores().ToLower();

            // try name with dashes
            yield return name.AddDashes();

            // try name with dashes with lower case
            yield return name.AddDashes().ToLower();

            // try name with underscore prefix
            yield return name.AddUnderscorePrefix();

            // try name with underscore prefix, using camel case
            yield return name.ToCamelCase(culture).AddUnderscorePrefix();
        }

        /// <summary>
        /// Parses most common JSON date formats
        /// </summary>
        /// <param name="input">JSON value to parse</param>
        /// <param name="culture"></param>
        /// <returns>DateTime</returns>
        public static DateTime ParseJsonDate(this string input, CultureInfo culture)
        {
            input = input.Replace("\n", "");
            input = input.Replace("\r", "");

            input = input.RemoveSurroundingQuotes();

            long unix;
            if (Int64.TryParse(input, out unix))
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddSeconds(unix);
            }

            if (input.Contains("/Date("))
            {
                return ExtractDate(input, @"\\?/Date\((-?\d+)(-|\+)?([0-9]{4})?\)\\?/", culture);
            }

            if (input.Contains("new Date("))
            {
                input = input.Replace(" ", "");
                // because all whitespace is removed, match against newDate( instead of new Date(
                return ExtractDate(input, @"newDate\((-?\d+)*\)", culture);
            }

            return ParseFormattedDate(input, culture);
        }

        /// <summary>
        /// Remove leading and trailing " from a string
        /// </summary>
        /// <param name="input">String to parse</param>
        /// <returns>String</returns>
        public static string RemoveSurroundingQuotes(this string input)
        {
            if (input.StartsWith("\"") && input.EndsWith("\""))
            {
                // remove leading/trailing quotes
                input = input.Substring(1, input.Length - 2);
            }
            return input;
        }

        private static DateTime ParseFormattedDate(string input, CultureInfo culture)
        {
            var formats = new[] {
                "u", 
                "s", 
                "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", 
                "yyyy-MM-ddTHH:mm:ssZ", 
                "yyyy-MM-dd HH:mm:ssZ", 
                "yyyy-MM-ddTHH:mm:ss", 
                "yyyy-MM-ddTHH:mm:sszzzzzz",
                "M/d/yyyy h:mm:ss tt" // default format for invariant culture
            };

            DateTime date;
            if (DateTime.TryParseExact(input, formats, culture, DateTimeStyles.None, out date))
            {
                return date;
            }

            if (DateTime.TryParse(input, culture, DateTimeStyles.None, out date))
            {
                return date;
            }

            return default(DateTime);
        }

        private static DateTime ExtractDate(string input, string pattern, CultureInfo culture)
        {
            DateTime dt = DateTime.MinValue;
            var regex = new Regex(pattern);
            if (regex.IsMatch(input))
            {
                var matches = regex.Matches(input);
                var match = matches[0];
                var ms = Convert.ToInt64(match.Groups[1].Value);
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                dt = epoch.AddMilliseconds(ms);

                // adjust if time zone modifier present
                if (match.Groups.Count > 2 && !String.IsNullOrEmpty(match.Groups[3].Value))
                {
                    var mod = DateTime.ParseExact(match.Groups[3].Value, "HHmm", culture);
                    if (match.Groups[2].Value == "+")
                    {
                        dt = dt.Add(mod.TimeOfDay);
                    }
                    else
                    {
                        dt = dt.Subtract(mod.TimeOfDay);
                    }
                }

            }
            return dt;
        }

        /// <summary>
        /// Converts a string to pascal case
        /// </summary>
        /// <param name="lowercaseAndUnderscoredWord">String to convert</param>
        /// <param name="culture"></param>
        /// <returns>string</returns>
        public static string ToPascalCase(this string lowercaseAndUnderscoredWord, CultureInfo culture)
        {
            return ToPascalCase(lowercaseAndUnderscoredWord, true, culture);
        }

        /// <summary>
        /// Converts a string to pascal case with the option to remove underscores
        /// </summary>
        /// <param name="text">String to convert</param>
        /// <param name="removeUnderscores">Option to remove underscores</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string text, bool removeUnderscores, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            text = text.Replace("_", " ");
            string joinString = removeUnderscores ? String.Empty : "_";
            string[] words = text.Split(' ');
            if (words.Length > 1 || words[0].IsUpperCase())
            {
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length > 0)
                    {
                        string word = words[i];
                        string restOfWord = word.Substring(1);

                        if (restOfWord.IsUpperCase())
                            restOfWord = restOfWord.ToLower();

                        char firstChar = char.ToUpper(word[0], culture);
                        words[i] = String.Concat(firstChar, restOfWord);
                    }
                }
                return String.Join(joinString, words);
            }
            return String.Concat(words[0].Substring(0, 1).ToUpper(), words[0].Substring(1));
        }

        /// <summary>
        /// Converts a string to camel case
        /// </summary>
        /// <param name="lowercaseAndUnderscoredWord">String to convert</param>
        /// <param name="culture"></param>
        /// <returns>String</returns>
        public static string ToCamelCase(this string lowercaseAndUnderscoredWord, CultureInfo culture)
        {
            return MakeInitialLowerCase(ToPascalCase(lowercaseAndUnderscoredWord, culture));
        }

        /// <summary>
        /// Convert the first letter of a string to lower case
        /// </summary>
        /// <param name="word">String to convert</param>
        /// <returns>string</returns>
        public static string MakeInitialLowerCase(this string word)
        {
            return String.Concat(word.Substring(0, 1).ToLower(), word.Substring(1));
        }

        /// <summary>
        /// Checks to see if a string is all uppper case
        /// </summary>
        /// <param name="inputString">String to check</param>
        /// <returns>bool</returns>
        public static bool IsUpperCase(this string inputString)
        {
            return Regex.IsMatch(inputString, @"^[A-Z]+$");
        }

        /// <summary>
        /// Add underscores to a pascal-cased string
        /// </summary>
        /// <param name="pascalCasedWord">String to convert</param>
        /// <returns>string</returns>
        public static string AddUnderscores(this string pascalCasedWord)
        {
            return
                Regex.Replace(
                    Regex.Replace(Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])",
                        "$1_$2"), @"[-\s]", "_");
        }

        /// <summary>
        /// Add dashes to a pascal-cased string
        /// </summary>
        /// <param name="pascalCasedWord">String to convert</param>
        /// <returns>string</returns>
        public static string AddDashes(this string pascalCasedWord)
        {
            return
                Regex.Replace(
                    Regex.Replace(
                        Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1-$2"),
                    @"([a-z\d])([A-Z])",
                "$1-$2"), @"[\s]", "-");
        }

        /// <summary>
        /// Add an undescore prefix to a pascasl-cased string
        /// </summary>
        /// <param name="pascalCasedWord"></param>
        /// <returns></returns>
        public static string AddUnderscorePrefix(this string pascalCasedWord)
        {
            return string.Format("_{0}", pascalCasedWord);
        }

        /// <summary>
        /// Converts an object from one type to another
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newType"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static object ChangeType(this object source, Type newType, CultureInfo culture)
        {
            return Convert.ChangeType(source, newType, culture);
        }

        /// <summary>
        /// Find a value from a System.Enum by trying several possible variants
        /// of the string value of the enum.
        /// </summary>
        /// <param name="type">Type of enum</param>
        /// <param name="value">Value for which to search</param>
        /// <param name="culture">The culture used to calculate the name variants</param>
        /// <returns></returns>
        public static object FindEnumValue(this Type type, string value, CultureInfo culture)
        {
            var ret = Enum.GetValues( type )
            .Cast<Enum>()
            .FirstOrDefault(v => v.ToString().GetNameVariants(culture).Contains(value, StringComparer.Create(culture, true)));

            if (ret == null)
            {
                var enumValueAsUnderlyingType = Convert.ChangeType(value, Enum.GetUnderlyingType(type), culture);
                if (enumValueAsUnderlyingType != null && Enum.IsDefined(type, enumValueAsUnderlyingType))
                {
                    ret = (Enum) Enum.ToObject(type, enumValueAsUnderlyingType);
                }
            }

            return ret;
        }

    }
}
