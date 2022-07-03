using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// 隐藏部分字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToCover(this string s, string c = "*")
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(s)) return "";
            if (string.IsNullOrEmpty(c)) return c;

            switch (s.Length)
            {
                case 1: return s;
                case 2: return s.Substring(0, 1) + c;
                default:
                    if (s.Length > 10) return s.Substring(0, 3).PadRight(s.Length - 4, '*') + s.Substring(s.Length - 4, 4);
                    else if (s.Length > 4) return s.Substring(0, 2).PadRight(s.Length - 2, '*') + s.Substring(s.Length - 2, 2);
                    else return s.Substring(0, 1) + c + s.Substring(s.Length - 1, 1);
            }
        }

        /// <summary>
        /// 从身份证获取出生年月日
        /// </summary>
        /// <param name="idcardno"></param>
        /// <returns></returns>
        public static DateTime GetBirthDay(this string idcardno)
        {
            // TODO
            return DateTime.Now;
        }

        /// <summary>
        /// 从身份证获取性别
        /// </summary>
        /// <param name="idcardno"></param>
        /// <returns></returns>
        public static byte GetGender(this string idcardno)
        {
            // TODO
            return 1;
        }

        public static string FromBase64(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            return Encoding.Default.GetString(Convert.FromBase64String(source));
        }

        public static string FromGBToUTF8(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            return Encoding.GetEncoding("GB2312").GetString(Encoding.UTF8.GetBytes(source));
        }

        public static string FromHex(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            byte[] bytes = new byte[source.Length / 2];
            for (int i = 0; i < source.Length; i += 2)
            {
                Convert.ToInt32(source.Substring(i, 2), 0x10).ToString();
                bytes[i / 2] = Convert.ToByte(source.Substring(i, 2), 0x10);
            }
            return Encoding.Default.GetString(bytes);
        }

        public static string FromUTF8ToGB(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            return Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(source));
        }

        private static string GetChineseSpell(string strText)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return string.Empty;
            }
            int length = strText.Length;
            string str = "";
            for (int i = 0; i < length; i++)
            {
                str = str + getSpell(strText.Substring(i, 1));
            }
            return str;
        }

        public static string GetPinYin(this string source)
        {
            return GetChineseSpell(source);
        }
        public static int GetRealLength(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return 0;
            }
            return Encoding.Default.GetByteCount(source);
        }

        private static string getSpell(string cnChar)
        {
            if (string.IsNullOrEmpty(cnChar))
            {
                return string.Empty;
            }
            byte[] bytes = Encoding.Default.GetBytes(cnChar);
            if (bytes.Length <= 1)
            {
                return cnChar;
            }
            int num = bytes[0];
            int num2 = bytes[1];
            int num3 = (num << 8) + num2;
            int[] numArray = new int[] {
            0xb0a1, 0xb0c5, 0xb2c1, 0xb4ee, 0xb6ea, 0xb7a2, 0xb8c1, 0xb9fe, 0xbbf7, 0xbbf7, 0xbfa6, 0xc0ac, 0xc2e8, 0xc4c3, 0xc5b6, 0xc5be,
            0xc6da, 0xc8bb, 0xc8f6, 0xcbfa, 0xcdda, 0xcdda, 0xcdda, 0xcef4, 0xd1b9, 0xd4d1
        };
            for (int i = 0; i < 0x1a; i++)
            {
                int num5 = 0xd7fa;
                if (i != 0x19)
                {
                    num5 = numArray[i + 1];
                }
                if ((numArray[i] <= num3) && (num3 < num5))
                {
                    return Encoding.Default.GetString(new byte[] { (byte)(0x41 + i) });
                }
            }
            return "*";
        }

        public static bool IsInteger(this string source)
        {
            int num;
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }
            return int.TryParse(source, out num);
        }

        public static bool IsNullOrEmpty(this string source)
        {
            if (source != null)
            {
                return (source.Trim().Length < 1);
            }
            return true;
        }

        public static string NarrowToBig(this string inputString)
        {
            char[] chars = inputString.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(chars, i, 1);
                if ((bytes.Length == 2) && (bytes[1] == 0))
                {
                    bytes[0] = (byte)(bytes[0] - 0x20);
                    bytes[1] = 0xff;
                    chars[i] = Encoding.Unicode.GetChars(bytes)[0];
                }
            }
            return new string(chars);
        }

        public static string NarrowToSmall(this string inputString)
        {
            char[] chars = inputString.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(chars, i, 1);
                if ((bytes.Length == 2) && (bytes[1] == 0xff))
                {
                    bytes[0] = (byte)(bytes[0] + 0x20);
                    bytes[1] = 0;
                    chars[i] = Encoding.Unicode.GetChars(bytes)[0];
                }
            }
            return new string(chars);
        }

        public static string RemoveNonPrintChars(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            Regex regex = new Regex("[\0-\b\v\f\x000e-\x001f]");
            return regex.Replace(source, "");
        }

        public static string ReplaceNonValidChars(this string filenameNoDir, string replaceWith)
        {
            if (string.IsNullOrEmpty(filenameNoDir))
            {
                return string.Empty;
            }
            return Regex.Replace(filenameNoDir, "[\\<\\>\\/\\\\\\|\\:\"\\*\\?\\r\\n]", replaceWith, RegexOptions.Compiled);
        }

        public static string SubStr(this string source, int resultLength)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            if (source.GetRealLength() <= resultLength)
            {
                return source;
            }
            return (source.SubString(resultLength) + "...");
        }

        public static string SubString(this string source, int resultLength)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            if (Encoding.Default.GetByteCount(source) > resultLength)
            {
                int num = 0;
                int length = 0;
                foreach (char ch in source)
                {
                    if (ch > '\x007f')
                    {
                        num += 2;
                    }
                    else
                    {
                        num++;
                    }
                    if (num > resultLength)
                    {
                        source = source.Substring(0, length);
                        return source;
                    }
                    length++;
                }
            }
            return source;
        }

        public static string ToBase64(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            return Convert.ToBase64String(Encoding.Default.GetBytes(source));
        }

        public static bool ToBoolean(this string source)
        {
            bool flag;
            return ((!string.IsNullOrEmpty(source) && bool.TryParse(source, out flag)) && flag);
        }

        public static DateTime ToDateTime(this string source)
        {
            return source.ToDateTime(DateTime.Now);
        }

        public static DateTime ToDateTime(this string source, DateTime defaultValue)
        {
            DateTime time;
            if (!string.IsNullOrEmpty(source) && DateTime.TryParse(source, out time))
            {
                return time;
            }
            return defaultValue;
        }
        public static DateTime? ToDateTime(this string source, DateTime? defaultValue)
        {
            DateTime time;
            if (!string.IsNullOrEmpty(source) && DateTime.TryParse(source, out time))
            {
                return time;
            }
            return defaultValue;
        }
        public static double ToDouble(this string source)
        {
            return source.ToDouble(-1.0);
        }


        public static double ToDouble(this string source, double defaultValue)
        {
            double num;
            if (!string.IsNullOrEmpty(source) && double.TryParse(source, out num))
            {
                return num;
            }
            return defaultValue;
        }

        public static T ToEnum<T>(this string source, T defaultValue)
        {
            if (!string.IsNullOrEmpty(source))
            {
                try
                {
                    T local = (T)Enum.Parse(typeof(T), source, true);
                    if (Enum.IsDefined(typeof(T), local))
                    {
                        return local;
                    }
                }
                catch
                {
                }
            }
            return defaultValue;
        }

        public static T ToEnumExt<T>(this string source, T defaultValue)
        {
            if (!string.IsNullOrEmpty(source))
            {
                try
                {
                    return (T)Enum.Parse(typeof(T), source, true);
                }
                catch
                {
                }
            }
            return defaultValue;
        }

        public static string ToEscapeRegChars(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            source = source.Replace("[", "[[]");
            source = source.Replace("%", "[%]");
            source = source.Replace("_", "[_]");
            source = source.Replace("^", "[^]");
            return source;
        }

        public static string ToHex(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            int length = source.Length;
            string str2 = "";
            byte[] bytes = new byte[2];
            for (int i = 0; i < length; i++)
            {
                int num2;
                string s = source.Substring(i, 1);
                bytes = Encoding.Default.GetBytes(s);
                int num5 = bytes.Length;
                if (num5.ToString() == "1")
                {
                    num2 = Convert.ToInt32(bytes[0]);
                    str2 = str2 + Convert.ToString(num2, 0x10);
                }
                else
                {
                    num2 = Convert.ToInt32(bytes[0]);
                    int num3 = Convert.ToInt32(bytes[1]);
                    str2 = str2 + Convert.ToString(num2, 0x10) + Convert.ToString(num3, 0x10);
                }
            }
            return str2.ToUpper();
        }

        public static int ToInt32(this string source)
        {
            return source.ToInt32(-1);
        }
        public static decimal ToDecimal(this string source, decimal defaultValue)
        {
            decimal num;
            if (!string.IsNullOrEmpty(source) && decimal.TryParse(source, out num))
            {
                return num;
            }
            return defaultValue;
        }

        public static int ToInt32(this string source, int defaultValue)
        {
            int num;
            if (!string.IsNullOrEmpty(source) && int.TryParse(source, out num))
            {
                return num;
            }
            return defaultValue;
        }

        public static long ToInt64(this string source)
        {
            return source.ToInt64(-1L);
        }

        public static long ToInt64(this string source, long defaultValue)
        {
            long num;
            if (!string.IsNullOrEmpty(source) && long.TryParse(source, out num))
            {
                return num;
            }
            return defaultValue;
        }

        public static string ToSafeJsString(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            source = source.Replace("'", @"\'");
            source = source.Replace("\"", "\\\"");
            source = source.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            return source;
        }

        public static string ToSafeSql(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            return source.Replace("'", "''");
        }

        public static string ToSafeXmlString(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            return source.Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        public static string ToUnicode(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(source.Substring(i, 1));
                if (bytes.Length > 1)
                {
                    string str = Convert.ToString((short)bytes[1], 0x10);
                    string str2 = Convert.ToString((short)bytes[0], 0x10);
                    str = ((str.Length == 1) ? "0" : "") + str;
                    str2 = ((str2.Length == 1) ? "0" : "") + str2;
                    builder.Append("&#" + Convert.ToInt32(str + str2, 0x10) + ";");
                }
            }
            return builder.ToString();
        }

        public static string ToUTF8(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(source.Substring(i, 1));
                if (bytes.Length > 1)
                {
                    string str = Convert.ToString((short)bytes[1], 0x10);
                    string str2 = Convert.ToString((short)bytes[0], 0x10);
                    builder.Append(@"\u" + (((str.Length == 1) ? "0" : "") + str) + (((str2.Length == 1) ? "0" : "") + str2));
                }
            }
            return builder.ToString();
        }

        public static string WrapWithCData(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            return string.Format("<![CDATA[{0}]]>", source);
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        public static string ToSentenceCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1], culture));
        }

        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }
        public static string JoinAsString(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }
        /// <summary>
        /// 分割字符串值
        /// </summary>
        /// <param name="instance">枚举实例</param>
        public static (string, string) GetSplitText(this string source)
        {
            var r = source.Split(',');
            if (r.Length == 2)
            {
                // 显示给后台看
                var adminShowText = r[0];
                // 显示给用户看
                var userShowText = r[1];
                return (adminShowText, userShowText);
            }
            return (source, source);

        }
        /// <summary>
        /// 指示指定的字符串是 null、<see cref="string.Empty"/> 还是仅由空白字符组成。
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns>如果 <paramref name="value"/> 参数为 null、<see cref="string.Empty"/> 或仅由空字符组成，则返回 true。 否则，false。</returns>
        //public static bool IsNullOrWhiteSpace(this string value)
        //{
        //    return string.IsNullOrWhiteSpace(value);
        //}
        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching object properties.
        /// <remarks>Uses <see cref="String.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="injectionObject">The object whose properties should be injected in the string</param>
        /// <returns>A version of the formatString string with keys replaced by (formatted) key values.</returns>
        public static string Inject(this string formatString, object injectionObject)
        {
            return formatString.Inject(GetPropertyHash(injectionObject));
        }
        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching hashtable entries.
        /// <remarks>Uses <see cref="String.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="attributes">A <see cref="Hashtable"/> with keys and values to inject into the string</param>
        /// <returns>A version of the formatString string with hastable keys replaced by (formatted) key values.</returns>
        public static string Inject(this string formatString, Hashtable attributes)
        {
            string result = formatString;
            if (attributes == null || formatString == null)
                return result;

            foreach (string attributeKey in attributes.Keys)
            {
                result = result.InjectSingleValue(attributeKey, attributes[attributeKey]);
            }
            return result;
        }
        /// <summary>
        /// Replaces all instances of a 'key' (e.g. {foo} or {foo:SomeFormat}) in a string with an optionally formatted value, and returns the result.
        /// </summary>
        /// <param name="formatString">The string containing the key; unformatted ({foo}), or formatted ({foo:SomeFormat})</param>
        /// <param name="key">The key name (foo)</param>
        /// <param name="replacementValue">The replacement value; if null is replaced with an empty string</param>
        /// <returns>The input string with any instances of the key replaced with the replacement value</returns>
        public static string InjectSingleValue(this string formatString, string key, object replacementValue)
        {
            string result = formatString;
            //regex replacement of key with value, where the generic key format is:
            //Regex foo = new Regex("{(foo)(?:}|(?::(.[^}]*)}))");
            Regex attributeRegex = new Regex("{(" + key + ")(?:}|(?::(.[^}]*)}))");  //for key = foo, matches {foo} and {foo:SomeFormat}

            //loop through matches, since each key may be used more than once (and with a different format string)
            foreach (Match m in attributeRegex.Matches(formatString))
            {
                string replacement = m.ToString();
                if (m.Groups[2].Length > 0) //matched {foo:SomeFormat}
                {
                    //do a double string.Format - first to build the proper format string, and then to format the replacement value
                    string attributeFormatString = string.Format(CultureInfo.InvariantCulture, "{{0:{0}}}", m.Groups[2]);
                    replacement = string.Format(CultureInfo.CurrentCulture, attributeFormatString, replacementValue);
                }
                else //matched {foo}
                {
                    replacement = (replacementValue ?? string.Empty).ToString();
                }
                //perform replacements, one match at a time
                result = result.Replace(m.ToString(), replacement);  //attributeRegex.Replace(result, replacement, 1);
            }
            return result;

        }
        /// <summary>
        /// Creates a HashTable based on current object state.
        /// <remarks>Copied from the MVCToolkit HtmlExtensionUtility class</remarks>
        /// </summary>
        /// <param name="properties">The object from which to get the properties</param>
        /// <returns>A <see cref="Hashtable"/> containing the object instance's property names and their values</returns>
        private static Hashtable GetPropertyHash(object properties)
        {
            Hashtable values = null;
            if (properties != null)
            {
                values = new Hashtable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(properties);
                foreach (PropertyDescriptor prop in props)
                {
                    values.Add(prop.Name, prop.GetValue(properties));
                }
            }
            return values;
        }

        public static string GetDBCondition(this string stringType)
        {
            string reslut = "";
            //switch (stringType?.ToLower())
            //{
            //    case HtmlElementType.droplist:
            //    case HtmlElementType.selectlist:
            //    case HtmlElementType.textarea:
            //    case HtmlElementType.checkbox:
            //        reslut = HtmlElementType.Contains;
            //        break;
            //    case HtmlElementType.thanorequal:
            //        reslut = HtmlElementType.ThanOrEqual;
            //        break;
            //    case HtmlElementType.lessorequal:
            //        reslut = HtmlElementType.LessOrequal;
            //        break;
            //    case HtmlElementType.gt:
            //        reslut = HtmlElementType.GT;
            //        break;
            //    case HtmlElementType.lt:
            //        reslut = HtmlElementType.lt;
            //        break;
            //    case HtmlElementType.like:
            //        reslut = HtmlElementType.like;
            //        break;
            //    default:
            //        reslut = HtmlElementType.Equal;
            //        break;
            //}
            return reslut;
        }

        public static long FormatUnixTime(this string value)
        {
            var unixVal = value;
            if (value.Length > 13)
                unixVal = value.Substring(0, value.Length - 6);
            long times = 0;
            long.TryParse(unixVal, out times);
            return times;
        }
        /// <summary>
        /// 字符串去除html格式
        ///     简易代码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string NoHtmlByJY(this string html)
        {
            string StrNohtml = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            StrNohtml = System.Text.RegularExpressions.Regex.Replace(StrNohtml, "&[^;]+;", "");
            return StrNohtml;
        }
        /// <summary>
        /// 字符串去除html格式
        ///     功能增强代码
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string NoHTML(this string Htmlstring)  //替换HTML标记
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return string.Empty;
            }
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = System.Web.HttpUtility.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

    }

}