using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace DashboardSite.Core.Utilities
{
    /// <summary>
    /// Text utils helper class
    /// </summary>
    public static class TextUtils
    {
        private static readonly char[] s_CommaSeparator = new char[] { ',' };


        /// <summary>
        /// Add string with separator
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="sValue">Value</param>
        /// <param name="sSeparator">Separator</param>
        /// <returns>StringBuilder</returns>
        public static StringBuilder AddWithSeparator(StringBuilder sb, string sValue, string sSeparator)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            if (string.IsNullOrEmpty(sValue))
                return sb;

            if (sb.Length > 0)
                sb.Append(sSeparator);
            sb.Append(sValue);
            return sb;
        }

        public static StringBuilder AddWithSeparatorVal<T>(StringBuilder sb, T value, string sSeparator)
            where T : struct
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            string s = value.ToString();
            if (string.IsNullOrEmpty(s))
                return sb;

            if (sb.Length > 0)
                sb.Append(sSeparator);
            sb.Append(s);
            return sb;
        }

        public static StringBuilder AddWithSeparatorObj<T>(StringBuilder sb, T value, string sSeparator)
            where T : class
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            string s = value != null ? value.ToString() : string.Empty;
            if (string.IsNullOrEmpty(s))
                return sb;

            if (sb.Length > 0)
                sb.Append(sSeparator);
            sb.Append(s);
            return sb;
        }

        /// <summary>
        /// Concatenate strings with separator
        /// </summary>
        /// <param name="s1">string 1</param>
        /// <param name="separator">separator</param>
        /// <param name="s2">string 2</param>
        /// <returns>concatenated string</returns>
        public static string Concat(string s1, string separator, string s2)
        {
            string sep = string.IsNullOrEmpty(s1) ? string.Empty : separator;
            return string.Concat(s1 ?? string.Empty, sep, s2 ?? string.Empty);
        }

        /// <summary>
        /// Add string with separator
        /// </summary>
        /// <param name="source">source string</param>
        /// <param name="sValue">Value</param>
        /// <param name="sSeparator">Separator</param>
        /// <returns>string</returns>
        public static string AddWithSeparator(string source, string sValue, string sSeparator)
        {
            string result = source ?? string.Empty;
            if (string.IsNullOrEmpty(sValue))
                return result;

            if (result.Length > 0)
                result += sSeparator;
            result += sValue;
            return result;
        }

        /// <summary>
        /// Is strning contains non-ws symbols
        /// </summary>
        /// <param name="str">String to check</param>
        /// <returns>true if <paramref name="str"/> contains any symbols except white characters</returns>
        public static bool IsNotEmpty(string str)
        {
            return !string.IsNullOrEmpty(str) && str.Trim().Length > 0;
        }

        /// <summary>
        /// Call ToString() if object not null
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>string</returns>
        public static string SafeToString(object obj)
        {
            return obj != null ? obj.ToString() : string.Empty;
        }

        /// <summary>
        /// Splits string formatted like "1,4,5,8,0".
        /// </summary>
        /// <param name="listIDs">string with ID's separated by commas</param>
        /// <returns>List of int's</returns>
        public static List<int> SplitStringToArray(string listIDs)
        {
            string[] IDs = (listIDs ?? string.Empty).Split(s_CommaSeparator, StringSplitOptions.RemoveEmptyEntries);
            List<int> result = new List<int>(IDs.Length);
            foreach (string id in IDs)
                result.Add(int.Parse(id.Trim()));
            return result;
        }

        /// <summary>
        /// Splits string formatted like "AFG,USA,UK", useful for Code
        /// </summary>
        /// <param name="listCodes">string with Code's separated by commas</param>
        /// <returns>List of code's</returns>
        public static List<string> SplitStringToArrayString(string listCodes)
        {
            if (String.IsNullOrEmpty(listCodes))
            {
                return new List<string>(); 
            }

            string[] Codes = listCodes.Split(s_CommaSeparator, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>(Codes.Length);
            foreach (string code in Codes)
                result.Add(code.Trim());
            return result;
        }


        /// <summary>
        /// Splits string formatted like "1,4,5,8,0".
        /// </summary>
        /// <param name="listIDs">string with ID's separated by commas</param>
        /// <returns>Dictinary with keys as IDs</returns>
        public static Dictionary<int, object> SplitStringToDictionaryKeys(string listIDs)
        {
            string[] IDs = listIDs.Split(s_CommaSeparator, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, object> result = new Dictionary<int, object>(IDs.Length);
            foreach (string id in IDs)
            {
                int intId = int.Parse(id);
                if (!result.ContainsKey(intId))
                    result.Add(intId, null);
            }
            return result;
        }

        /// <summary>
        /// TokenDelimitedText
        /// </summary>
        /// <param name="arrString"></param>
        /// <param name="sToken"></param>
        /// <returns></returns>
        public static string TokenDelimitedText(IEnumerable<string> arrString, string sToken)
        {
            if (arrString == null) throw new ArgumentNullException("arrString");

            StringBuilder oSB = new StringBuilder();
            bool bAppendToken = false;
            foreach (string str in arrString)
            {
                if (string.IsNullOrEmpty(str)) continue;

                if (bAppendToken)
                    oSB.Append(sToken);
                oSB.Append(str);
                bAppendToken = true;
            }

            return oSB.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sToken"></param>
        /// <returns></returns>
        public static string TokenDelimitedText(IEnumerable list, string sToken)
        {
            StringBuilder oSB = new StringBuilder();
            bool bAppendToken = false;
            foreach (object item in list)
            {
                string str = item.ToString();
                if (string.IsNullOrEmpty(str)) continue;

                if (bAppendToken)
                    oSB.Append(sToken);
                oSB.Append(str);
                bAppendToken = true;
            }
            return oSB.ToString();
        }


        public static string TokenDelimitedText<T>(IEnumerable list, string sToken, Func<T,string> oConvertDelegate) where T : class
        {
            StringBuilder oSB = new StringBuilder();
            bool bAppendToken = false;
            foreach (object obj in list)
            {
                T item = obj as T;

                if (item == null)
                {
                    continue;
                }

                string str = oConvertDelegate(item);
                if (string.IsNullOrEmpty(str)) continue;

                if (bAppendToken)
                    oSB.Append(sToken);
                oSB.Append(str);
                bAppendToken = true;
            }
            return oSB.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sToken"></param>
        /// <returns></returns>
        public static string TokenDelimitedText(IEnumerable<int> list, string sToken)
        {
            StringBuilder oSB = new StringBuilder();
            bool bAppendToken = false;
            foreach (int item in list)
            {
                string str = item.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    if (bAppendToken)
                        oSB.Append(sToken);
                    oSB.Append(str);
                    bAppendToken = true;
                }
            }
            return oSB.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrString"></param>
        /// <returns></returns>
        [Obsolete("Use CommaDelimitedText instead of the misspelled")]
        public static string CommaDemiltedText(IEnumerable<string> arrString)
        {
            return CommaDelimitedText(arrString);
        }

        public static string CommaDelimitedText(IEnumerable<string> arrString)
        {
            return TokenDelimitedText(arrString, ",");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Obsolete("Use CommaDelimitedText instead of the misspelled")]
        public static string CommaDemiltedText(IEnumerable list)
        {
            return CommaDelimitedText(list);
        }

        public static string CommaDelimitedText(IEnumerable list)
        {
            return TokenDelimitedText(list, ",");
        }

        public static string CommaDelimitedText(IEnumerable<int> list)
        {
            return TokenDelimitedText(list, ",");
        }

        public static string TextWidthController(string sText, int iOffset, int iMaxLength)
        {
            if (sText.Length > iMaxLength)
            {
                while (iOffset > 0)
                {
                    if (sText[iOffset] != ' ' && sText[iOffset] != '\n' && sText[iOffset] != '\r')
                        iOffset--;
                    else break;
                }
                if (iOffset < 0)
                {
                    iOffset = 0;
                }
                int iEnd = iOffset;
                while ((iEnd - iOffset) < iMaxLength)
                {
                    iEnd = sText.IndexOfAny(new char[] { ' ', '\n', '\r', '\t' }, iEnd + 1);
                    if (iEnd < 0)
                    {
                        iEnd = sText.Length;
                        break;
                    }

                    if ((iEnd - iOffset) > iMaxLength * 2)
                    {
                        iEnd = iMaxLength + iOffset;
                    }
                }
                return string.Format("{0}{1}{2}", ((iOffset == 0 ? string.Empty : "...")), sText.Substring(iOffset, iEnd - iOffset), ((iEnd < sText.Length ? "..." : string.Empty)));
            }
            return sText;
        }

        public static string[] ListToStringArray<T>(List<T> oList)
        {
            ArrayList oAL = new ArrayList();

            foreach (T item in oList)
                oAL.Add(item.ToString());

            return (string[]) oAL.ToArray(typeof(string));
        }

        public static string ReplaceBrackets(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            return value.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public static string EncodeBase64(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in EncodeBase64 " + e.Message);
            }
        }

        public static string DecodeBase64(string data)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in DecodeBase64 " + e.Message);
            }
        }

        public static int? ParseIntSafe(string intValue)
        {
            int result;
            if (int.TryParse(intValue, out result))
                return result;
            return null;
        }

        public static string TrimRightSafe(string text, int length, string appendix)
        {
            string result = text;
            if (text.Length > length)
            {
                result = text.Substring(0, length - 1);
                if (!string.IsNullOrEmpty(appendix))
                    result += appendix;
            }
            return result;
        }

        public static string TrimExtraSpaces(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            int startLength, endLength;
            StringBuilder result = new StringBuilder(text);
            while(true)
            {
                startLength = result.Length;
                result = result.Replace("  ", " ");
                endLength = result.Length;
                if (startLength == endLength)
                    break;
            }
            while (true)
            {
                startLength = result.Length;
                result = result.Replace("\n\n", "\n");
                endLength = result.Length;
                if (startLength == endLength)
                    break;
            }
            return result.ToString();
        }

        public static string GetFirstNLines(string text, int linesCount)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            int startIndex = 0, curIndex = 0;
            for(int i = 0; i < linesCount; i++)
            {
                curIndex = text.IndexOf("\n", startIndex);
                if (curIndex < 0)
                {
                    curIndex = text.Length - 1;
                    break;
                }
                startIndex = curIndex + 1;
            }
            string result = text.Substring(0, curIndex);
            return result;
        }

        #region Html related
        
        const string HTML_TAG_PATTERN = "<.*?>";

        public static string StripHTML(string inputString)
        {
            if (inputString != null)
                return System.Text.RegularExpressions.Regex.Replace
                  (inputString, HTML_TAG_PATTERN, string.Empty);
            else
                return inputString;
        }


        #endregion
    }
}
