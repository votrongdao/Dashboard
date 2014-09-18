using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace DashboardSite.Core.Utilities
{
    public class XmlHelper
    {
        public static string Serialize(object source, Type type, Encoding encoding)
        {
            // The string to hold the object content
            String content;

            // Create a memoryStream into which the data can be written and readed
            using (var stream = new MemoryStream())
            {
                // Create the xml serializer, the serializer needs to know the type
                // of the object that will be serialized
                var xmlSerializer = new XmlSerializer(type);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = encoding;
                settings.OmitXmlDeclaration = true;
                //using (var writer = new XmlTextWriter(stream, encoding))
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    // Save the state of the object into the stream
                    xmlSerializer.Serialize(writer, source);

                    // Flush the stream
                    writer.Flush();

                    // Read the stream into a string
                    using (var reader = new StreamReader(stream, encoding))
                    {
                        // Set the stream position to the begin
                        stream.Position = 0;

                        // Read the stream into a string
                        content = reader.ReadToEnd();
                    }
                }
            }

            // Return the xml string with the object content
            return content;
        }

        static public Object Deserialize(Type type, String pXmlizedString, Encoding encoding)
        {
            using (var stream = new MemoryStream(encoding.GetBytes(pXmlizedString)))
            {
                var xmlSerializer = new XmlSerializer(type);
                return xmlSerializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="pObject">Object that is to be serialized to XML</param>
        /// <returns>XML string</returns>
        static public String SerializeObject(Object pObject)
        {
            String XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(pObject.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, pObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return XmlizedString;
        }

        /// <summary>
        /// Method to deseriailize and xml string back to an object.
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="pXmlizedString"></param>
        /// <returns></returns>
        static public Object DeserializeObject(Type objType, String pXmlizedString)
        {
            XmlSerializer xs = new XmlSerializer(objType);
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return xs.Deserialize(memoryStream);
        }

        /// <summary>
        /// Convert token delimited string into xml string
        /// </summary>
        /// <param name="sRootName">XML root node name</param>
        /// <param name="sSource">delimited string</param>
        /// <param name="cSeperator">seperator token</param>
        /// <param name="bOuterXml">if true, return full xml including the root node, or the inner xml without root node</param>
        /// <returns></returns>
        static public string ConvertDelimitedStringIntoXml(string sRootName, string sSource, char[] cSeperator, bool bOuterXml)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml("<" + sRootName + "/>");
            XmlNode xRoot = xDoc.DocumentElement;
            string[] strArray = sSource.Split(cSeperator);
            foreach (string str in strArray)
            {
                XmlElement xElem = xDoc.CreateElement("item");
                xElem.InnerText = str;
                xRoot.AppendChild(xElem);
            }
            if (bOuterXml)
                return xDoc.OuterXml;
            return xDoc.DocumentElement.InnerXml;
        }

        /// <summary>
        /// Convert set of items to XML
        /// </summary>
        /// <param name="sRootName">Root item name</param>
        /// <param name="items">items</param>
        /// <param name="bOuterXml">add root item</param>
        /// <returns>XML
        /// <code>
        /// <root><item name="item-value"></item></root>
        /// </code>
        /// </returns>
        public static StringBuilder ConvertToXmlAttributeCentric(string sRootName, IEnumerable<string> items, bool bOuterXml)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (string.IsNullOrEmpty(sRootName))
                throw new ArgumentNullException("sRootName");

            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.OmitXmlDeclaration = !bOuterXml;

            using (XmlWriter xmlw = XmlWriter.Create(sb, settings))
            {
                if (bOuterXml)
                {
                    xmlw.WriteStartElement(sRootName);
                }

                foreach (string s in items)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    xmlw.WriteStartElement("item");
                    xmlw.WriteAttributeString("name", s);
                    xmlw.WriteEndElement();
                }

                if (bOuterXml)
                    xmlw.WriteEndElement();
            }
            return sb;
        }

        /// <summary>
        /// Convert set of items to XML
        /// </summary>
        /// <param name="sRootName">Root item name</param>
        /// <param name="items">items</param>
        /// <param name="bOuterXml">add root item</param>
        /// <returns>XML
        /// <code>
        /// <root><item name="item-value"></item></root>
        /// </code>
        /// </returns>
        public static StringBuilder ConvertToXmlAttributeCentric(string sRootName, IEnumerable<int> items, bool bOuterXml)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            List<string> strItems = new List<string>();
            foreach (int item in items)
                strItems.Add(item.ToString());
            return ConvertToXmlAttributeCentric(sRootName, strItems, bOuterXml);
        }

        static public string ConvertDelimitedStringIntoXmlAttributeCentric(string sRootName, string sSource, char[] cSeperator, bool bOuterXml)
        {
            string[] strArray = sSource.Split(cSeperator);
            return ConvertToXmlAttributeCentric(sRootName, strArray, bOuterXml).ToString();
        }

        #region Private methods

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        static private String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        static private Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        #endregion Private methods

        #region Pseudo-Xml

        private const string XmlOpen = "<";
        private const string PXmlOpen = "[[";
        private const string XmlClose = ">";
        private const string PXmlClose = "]]";
        private const string XmlCloseTag = "/>";
        private const string PXmlCloseTag = "!]]";
        private const string XmlEndTag = "</";
        private const string PXmlEndTag = "[[!";

        public static string PXmlEncode(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return string.Empty;

            StringBuilder sb = new StringBuilder(xml);
            return PXmlEncode(sb).ToString();
        }

        public static StringBuilder PXmlEncode(StringBuilder sb)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            sb.Replace(XmlCloseTag, PXmlCloseTag);
            sb.Replace(XmlEndTag, PXmlEndTag);
            sb.Replace(XmlOpen, PXmlOpen);
            sb.Replace(XmlClose, PXmlClose);
            return sb;
        }

        public static string PXmlDecode(string xml, bool withCDATA)
        {
            if (string.IsNullOrEmpty(xml))
                return string.Empty;

            StringBuilder sb = new StringBuilder(xml);
            return PXmlDecode(sb, withCDATA);
        }
        
        public static string PXmlDecode(string xml)
        {
            return PXmlDecode(xml,true);
        }
        
        public static string PXmlDecode(StringBuilder sb)
        {
            return PXmlDecode(sb,true);
        }

        public static string PXmlDecode(StringBuilder sb, bool withCDATA)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            sb.Replace(PXmlCloseTag, XmlCloseTag);
            sb.Replace(PXmlEndTag, XmlEndTag);
            sb.Replace(PXmlOpen, XmlOpen);
            sb.Replace(PXmlClose, XmlClose);

            string result = sb.ToString();
            if(withCDATA)
            {
                MatchEvaluator replacer = StartElementReplacer;
                Regex rx = new Regex("\\<\\b[a-z0-9_]+\\b\\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                result = rx.Replace(result, replacer);

                replacer = EndElementReplacer;
                rx = new Regex("\\</\\b[a-z0-9_]+\\b\\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                result = rx.Replace(result, replacer);
            }

            return result;
        }

        private static string StartElementReplacer(Match match)
        {
            return string.Format("{0}<![CDATA[", match.Value);
        }

        private static string EndElementReplacer(Match match)
        {
            return string.Format("]]>{0}", match.Value);
        }

        #endregion Pseudo-Xml
    }
}
