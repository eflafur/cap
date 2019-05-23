using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace GruppoCap
{
    public static class StringUtils
    {

        // CONSTs
        private static readonly CultureInfo _InvariantCulture = CultureInfo.InvariantCulture;

        private static readonly Regex _Regex_MultipleWhiteSpacesNonLineBreaks = new Regex(@"[ \t]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static readonly Regex _Regex_MultipleWhiteSpacesWithLineBreaks = new Regex(@"[\s]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static readonly Regex _Regex_Hyphenization = new Regex(@"[-\s]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static readonly Regex _Regex_OnlyBasicChars = new Regex(@"[^a-zA-Z_0-9\-]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static readonly Regex _Regex_FirstUppercaseLetterOfWordsFinder = new Regex(@"(?<=[^\b])[A-Z]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);
        private static readonly Regex _Regex_Tags = new Regex(@"<[^>]+>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        // IS NULL OR WHITE SPACE
        public static Boolean IsNullOrWhiteSpace(this String s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        // IF NULL THEN
        public static String IfNullThen(this String s, String defaultValue = "")
        {
            return s ?? defaultValue;
        }

        // IF NULL OR WHITESPACE THEN
        public static String IfNullOrWhiteSpaceThen(this String s, String defaultValue = "")
        {
            if (s.IsNullOrWhiteSpace())
                return defaultValue;

            return s;
        }

        // BOOLEAN TO STRING
        public static String ToString(this Boolean b, String trueValue, String falseValue)
        {
            return b ? trueValue : falseValue;
        }

        // BOOLEAN TO STRING YN
        public static String ToStringYN(this Boolean b)
        {
            return b.ToString("Y", "N");
        }

        // BOOLEAN TO STRING 0/1
        public static String ToString01(this Boolean b)
        {
            return b.ToString("1", "0");
        }

        // REMOVE NULLIES
        public static String[] RemoveNullsOrWhiteSpaces(this IEnumerable<String> strings)
        {
            if (strings.IsNullOrEmpty())
                return strings.ToArray();

            return strings.Where(s => s.IsNullOrWhiteSpace() == false).ToArray();
        }

        // REMOVE TAGS
        public static String RemoveTags(this String taggedString)
        {
            if (taggedString.IsNullOrWhiteSpace())
                return String.Empty;

            String cleanString = _Regex_Tags.Replace(taggedString, string.Empty);
            return cleanString;
        }

        // EQUALS RELAXED
        public static Boolean EqualsRelaxed(this String source, String target)
        {
            return String.Equals(source, target, StringComparison.InvariantCultureIgnoreCase);
        }

        // EQUALS RELAXED ANY
        public static Boolean EqualsRelaxedAny(this String source, params String[] targets)
        {
            if (targets.Length == 0)
                return false;

            return targets.Any(s => s.EqualsRelaxed(source));
        }

        // FORMAT WITH
        public static String FormatWith(this String format, params Object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            return String.Format(format, args);
        }

        // JOIN
        public static String Join(this IEnumerable<String> strings, String separator)
        {
            if (strings.IsNullOrEmpty())
                return String.Empty;

            return String.Join(separator, strings);
        }

        // CONCAT
        public static String Combine(this String originalString, String concatString)
        {
            if (originalString.IsNullOrWhiteSpace())
                return concatString;

            return String.Concat(originalString, concatString);
        }

        // TO TITLE CASE
        public static String ToTitleCase(this String s, TextInfo ti = null)
        {
            if (s == null)
                return null;

            if (ti == null)
                ti = _InvariantCulture.TextInfo;

            return ti.ToTitleCase(s);
        }

        // TO UPPER CASE - THE SAFE WAY
        public static String ToUpperCase(this String s, TextInfo ti = null)
        {
            if (s == null)
                return null;

            if (ti == null)
                ti = _InvariantCulture.TextInfo;

            return ti.ToUpper(s);
        }

        // UPPERCASE FIRST LETTER OF WORDs
        public static String UppercaseFirstLetterOfWords(this String s, CultureInfo ci = null)
        {
            if (String.IsNullOrWhiteSpace(s))
                return s;

            if (ci == null)
                ci = _InvariantCulture;

            Char[] chars = s.ToCharArray();

            if (chars.Length >= 1)
            {
                if (Char.IsLower(chars[0]))
                {
                    chars[0] = Char.ToUpper(chars[0], ci);
                }
            }

            for (Int32 i = 1; i < chars.Length; i++)
            {
                if (Char.IsWhiteSpace(chars[i - 1])) // && Char.IsLower(chars[i]))
                {
                    chars[i] = Char.ToUpper(chars[i], ci);
                }
                else
                {
                    chars[i] = Char.ToLower(chars[i], ci);
                }

            }

            return new String(chars);
        }

        // UPPERCASE FIRST CHAR ONLY
        public static String UppercaseFirstCharOnly(this String s, Boolean lowercaseTheRest = true)
        {
            if (s.IsNullOrWhiteSpace())
            {
                return s;
            }

            if (s.Length <= 1)
            {
                return s.ToUpperInvariant();
            }

            s = s.ToLowerInvariant();

            return s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
        }

        // TRUNCATE
        public static String Truncate(this String s, Int32 maxLength, Boolean addEllipsis = false, Boolean addEllipsisAtTheCenter = false, Boolean ellipsisAsHtml = false)
        {
            // NULL OR EMPTY
            if (String.IsNullOrWhiteSpace(s))
                return s;

            // SHORTER THAN MAXLENGTH
            if (s.Length <= maxLength)
                return s;

            String ellipsis;
            Int32 ellipsisLogicalLength;

            if (ellipsisAsHtml)
            {
                ellipsis = "&hellip;";
                ellipsisLogicalLength = 1;
            }
            else
            {
                ellipsis = "...";
                ellipsisLogicalLength = 3;
            }

            // MAXLENGTH LOWER THAN ELLIPSIS LOGICAL LENGTH
            if (addEllipsis && maxLength <= ellipsisLogicalLength)
                return s.Substring(0, maxLength);

            if (addEllipsis == false)
            {
                // SIMPLE TRUNCATE
                return s.Substring(0, maxLength);
            }

            if (addEllipsisAtTheCenter == false)
            {
                // ADD THE ELLIPSIS AT THE END
                Int32 l;

                l = addEllipsis ? maxLength - ellipsisLogicalLength : maxLength;

                return s.Substring(0, l) + ellipsis;
            }

            // ADD THE FAKE ELLIPSIS AT THE CENTER
            Int32 amountOfCharsToTake, leftPartLength, rightPartLength;

            amountOfCharsToTake = maxLength - ellipsisLogicalLength;

            rightPartLength = Convert.ToInt32(amountOfCharsToTake / 2);
            leftPartLength = amountOfCharsToTake - rightPartLength;

            return
                s.Substring(0, leftPartLength)
                + ellipsis +
                s.Substring(s.Length - rightPartLength, rightPartLength)
            ;
        }

        // TRUNCATE TO
        public static String TruncateTo(this String s, String token)
        {
            if (s.IsNullOrWhiteSpace())
                return s;

            Int32 _pos;

            _pos = s.IndexOf(token);

            if (_pos < 0)
                return s;

            return s.Substring(0, _pos);
        }

        // ENSURE STARTS WITH
        public static String EnsureStartsWith(this String s, Char c)
        {
            if (String.IsNullOrWhiteSpace(s) || s[0] != c)
                return c + s;

            return s;
        }

        // TRIM START
        public static String TrimStart(this String s, String c, StringComparison comparisonType)
        {
            if (s.IsNullOrWhiteSpace())
                return s;

            return s.StartsWith(c, comparisonType) ? s.Substring(c.Length) : s;
        }

        // TRIM START
        public static String TrimEnd(this String s, String c, StringComparison comparisonType)
        {
            if (s.IsNullOrWhiteSpace())
                return s;

            return s.EndsWith(c, comparisonType) ? s.Substring(0, s.Length - c.Length) : s;
        }

        // TRIM START RELAXED
        public static String TrimStartRelaxed(this String s, String c)
        {
            return s.TrimStart(c, StringComparison.InvariantCultureIgnoreCase);
        }

        // TRIM END RELAXED
        public static String TrimEndRelaxed(this String s, String c)
        {
            return s.TrimEnd(c, StringComparison.InvariantCultureIgnoreCase);
        }

        // ENSURE STARTS WITH
        public static String EnsureStartsWith(this String s, String prefix, StringComparison? comparison = null)
        {
            if (s.IsNullOrWhiteSpace())
                return prefix;

            if (s.StartsWith(prefix, comparison.GetValueOrDefault(StringComparison.InvariantCultureIgnoreCase)) == false)
                return prefix + s;

            return s;
        }

        // ENSURE ENDS WITH
        public static String EnsureEndsWith(this String s, Char c)
        {
            if (s.IsNullOrWhiteSpace() || s[s.Length - 1] != c)
                return s + c;

            return s;
        }

        // ENSURE ENDS IF NOT NULL WITH
        public static String EnsureEndsIfNotNullWith(this String s, Char c)
        {
            if (s.IsNullOrWhiteSpace())
                return String.Empty;

            return s.EnsureEndsWith(c);
        }

        // ENSURE ENDS WITH
        public static String EnsureEndsWith(this String s, String suffix, StringComparison? comparison = null)
        {
            if (s.IsNullOrWhiteSpace())
                return suffix;

            if (s.EndsWith(suffix, comparison.GetValueOrDefault(StringComparison.InvariantCultureIgnoreCase)) == false)
                return s + suffix;

            return s;
        }



        // ENSURE SURROUNDED BY
        public static String EnsureSurroundedBy(this String s, Char c)
        {
            return s.EnsureStartsWith(c).EnsureEndsWith(c);
        }

        // ENSURE SURROUNDED BY
        public static String EnsureSurroundedBy(this String s, String prefix, String suffix, StringComparison? comparison = null)
        {
            return s
                .EnsureStartsWith(prefix, comparison)
                .EnsureEndsWith(suffix, comparison)
            ;
        }

        // CONDENSE WHITESPACEs
        public static String CondenseWhiteSpaces(this String s, Boolean includeNewLines = false)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            if (includeNewLines)
            {
                return _Regex_MultipleWhiteSpacesWithLineBreaks.Replace(s, " ");
            }

            return _Regex_MultipleWhiteSpacesNonLineBreaks.Replace(s, " ");
        }

        // REMOVE WHITESPACEs
        public static String RemoveWhiteSpaces(this String s, Boolean includeNewLines = false)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            if (includeNewLines)
            {
                return _Regex_MultipleWhiteSpacesWithLineBreaks.Replace(s, String.Empty);
            }

            return _Regex_MultipleWhiteSpacesNonLineBreaks.Replace(s, String.Empty);
        }

        // COMBINE STRINGs WITH CHAR SEPARATOR
        public static String CombineStringsWithCharSeparator(Char separator, params String[] strings)
        {
            // CHECK - NULL
            if (strings == null || strings.Length == 0)
                return String.Empty;

            // REMOVE EMPTY ONEs
            strings = strings.Where(s => String.IsNullOrWhiteSpace(s) == false).ToArray();

            // CHECK - EMPTY
            if (strings.Length == 0)
                return String.Empty;

            // CHECK FOR ONLY 1 TOKEN PASSED
            if (strings.Length == 1)
                return strings[0];

            // COMBINE ALL THE TOKENs TOGHETER
            String s1 = String.Empty;

            foreach (String token in strings)
            {
                if (String.IsNullOrEmpty(s1))
                    s1 = token;
                else
                    s1 = s1.EnsureEndsWith(separator) + token.TrimStart(separator);
            }

            return s1;
        }

        // REPLACE
        public static String Replace(this String s, Regex r, String replacement)
        {
            if (s == null)
                return s;

            return r.Replace(s, replacement);
        }

        // REMOVE
        public static String Remove(this String s, String r)
        {
            if (s == null)
                return s;

            return s.Replace(r, "");
        }

        // REMOVE DIACRITICS
        public static String RemoveDiacritics(this String s)
        {
            if (String.IsNullOrWhiteSpace(s))
                return s;

            String normalized = null;
            StringBuilder sb = null;
            Char c = '\0';

            normalized = s.Normalize(NormalizationForm.FormD);

            sb = new StringBuilder(String.Empty);

            for (Int32 i = 0; i <= normalized.Length - 1; i++)
            {
                c = normalized[i];
                if ((CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) == false)
                    sb.Append(c);
            }

            return sb.ToString();
        }

        // HYPHENIZE
        public static String Hyphenize(this String s)
        {
            if (s == null)
                return s;

            return _Regex_Hyphenization.Replace(s, "-");
        }

        // SLUGIFY
        public static String Slugify(this String s, Int32? maxLength = null)
        {
            if (String.IsNullOrEmpty(s))
                return String.Empty;

            s = s
                .Trim()
                .RemoveDiacritics()
                .Hyphenize()
                .Replace(_Regex_OnlyBasicChars, String.Empty)
                .ToLowerInvariant()
            ;

            if (maxLength.GetValueOrDefault() > 0)
            {
                s = s.Truncate(maxLength.GetValueOrDefault());
            }

            return s;
        }

        // DETACH WORDs
        public static String DetachWords(this String s, Boolean toLowerCase = false)
        {
            return _Regex_FirstUppercaseLetterOfWordsFinder.Replace(s, (m) =>
            {
                if (toLowerCase)
                    return " " + m.Value.ToLowerInvariant();

                return " " + m.Value;
            });
        }

        // GENERATE SHA1 HASH
        public static String GenerateSHA1Hash(String s, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            Byte[] source, hash;
            SHA1CryptoServiceProvider sha1;

            sha1 = new SHA1CryptoServiceProvider();

            source = UTF8Encoding.UTF8.GetBytes(s);
            hash = sha1.ComputeHash(source);

            return Convert.ToBase64String(hash, options);
        }

        // GENERATE SHA256 HASH
        public static String GenerateSHA256Hash(String s, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            Byte[] source, hash;
            SHA256CryptoServiceProvider sha256;

            sha256 = new SHA256CryptoServiceProvider();

            source = UTF8Encoding.UTF8.GetBytes(s);
            hash = sha256.ComputeHash(source);

            return Convert.ToBase64String(hash, options);
        }

        // RIGHT
        public static String Right(this String str, int length)
        {
            if (str.IsNullOrWhiteSpace())
                return String.Empty;

            return (str.Length >= length)
                ? str.Substring(str.Length - length, length)
                : str;
        }

        // LEFT
        public static String Left(this String str, int length)
        {
            if (str.IsNullOrWhiteSpace())
                return String.Empty;

            return str.Substring(0, Math.Min(length, str.Length));
        }

        
    }
}
