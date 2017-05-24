using System;
using System.Collections.Generic;
using FSSHTTPandWOPIInspector.Parsers;
using System.Xml;
using System.Reflection;

namespace FSSHTTPandWOPIInspector
{
    /// <summary>
    /// The utilities class for FSSHTTPAndWOPI Inspector.
    /// </summary>
    class Utilities
    {
        /// <summary>
        /// Convert the data format from uint to string 
        /// </summary>
        /// <param name="data">The uint data</param>
        /// <returns>The converted string result</returns>
        public static string ConvertUintToString(uint data)
        {
            return data.ToString() + " (0x" + data.ToString("X8") + ")";
        }

        /// <summary>
        /// Convert the data format from ushort to string 
        /// </summary>
        /// <param name="data">The ushort data</param>
        /// <returns>The converted string result</returns>
        public static string ConvertUshortToString(ushort data)
        {
            return data.ToString() + " (0x" + data.ToString("X4") + ")";
        }

        /// <summary>
        /// Get the valid response from HTTP chunked response body.
        /// </summary>
        /// <param name="responseBodyFromFiddler">The raw response body from Fiddler.</param>
        /// <returns>The valid response bytes</returns>
        public static byte[] GetPaylodFromChunkedBody(byte[] responseBodyFromFiddler)
        {
            int length = responseBodyFromFiddler.Length;
            List<byte> payload = new List<byte>();

            int chunkSize;
            int i = 0;
            do
            {
                chunkSize = 0;
                while (true)
                {
                    int b = responseBodyFromFiddler[i];

                    if (b >= 0x30 && b <= 0x39)
                    {
                        b -= 0x30;
                    }
                    else if (b >= 0x41 && b <= 0x46)
                    {
                        b -= 0x41 - 10;
                    }
                    else if (b >= 0x61 && b <= 0x66)
                    {
                        b -= 0x61 - 10;
                    }
                    else
                    {
                        break;
                    }
                    chunkSize = chunkSize * 16 + b;
                    i++;
                }

                if (responseBodyFromFiddler[i] != 0x0D || responseBodyFromFiddler[i + 1] != 0x0A)
                {
                    throw new Exception();
                }
                i += 2;
                for (int k = 0; k < chunkSize; k++, i++)
                {
                    payload.Add(responseBodyFromFiddler[i]);
                }
                if (responseBodyFromFiddler[i] != 0x0D || responseBodyFromFiddler[i + 1] != 0x0A)
                {
                    throw new Exception();
                }
                i += 2;
            } while (chunkSize > 0);
            return payload.ToArray();
        }

        /// <summary>
        /// Determines whether two specified byte array have the same content.
        /// </summary>
        /// <param name="array1">The first byte array.</param>
        /// <param name="array2">The second byte array.</param>
        /// <returns>true if the content of array1 is the same as the content of array2; otherwise, false.</returns>
        public static bool ByteArrayEquals(byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null)
            {
                return true;
            }
            else if (array2.Length != array1.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// This method is used to test whether it is an editors table header.
        /// </summary>
        /// <param name="content">Specify the header content.</param>
        /// <returns>Return true if the content is editors table header, otherwise return false.</returns>
        public static bool IsEditorsTableHeader(byte[] content)
        {
            byte[] editorsTableHeaderTmp = new byte[8];

            if (content.Length < 8 || content.Length > 8)
            {
                return false;
            }

            Array.Copy(content, 0, editorsTableHeaderTmp, 0, 8);
            return ByteArrayEquals(EditorsTableHeader, editorsTableHeaderTmp);
        }


        /// <summary>
        /// This method is used to test whether it is an editors table header.
        /// </summary>
        /// <param name="content">Specify the header content.</param>
        /// <returns>Return true if the content is editors table header, otherwise return false.</returns>
        public static bool IsPNGHeader(byte[] content)
        {
            byte[] PNGHeaderTmp = new byte[8];

            if (content.Length < 8)
            {
                return false;
            }

            Array.Copy(content, 0, PNGHeaderTmp, 0, 8);
            return ByteArrayEquals(PNG, PNGHeaderTmp);
        }

        /// <summary>
        /// Check the input data is ZIP file header.
        /// </summary>
        /// <param name="byteArray">The content of a file.</param>
        /// <param name="expectedHeader">The expected header.</param>
        /// <returns>True if the input data is a local file header, otherwise false.</returns>
        public static bool IsZIPFileHeaderMatch(byte[] byteArray, byte[] expectedHeader)
        {
            if (byteArray.Length < 4)
            {
                return false;
            }
            byte[] localHeaderTmp = new byte[4];
            Array.Copy(byteArray, 0, localHeaderTmp, 0, 4);
            if (ByteArrayEquals(expectedHeader, localHeaderTmp))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Static field for the editors table header byte content.
        /// </summary>
        public static readonly byte[] EditorsTableHeader = new byte[] { 0x1a, 0x5a, 0x3a, 0x30, 0, 0, 0, 0 };

        /// <summary>
        /// The file header in zip.
        /// </summary>
        public static readonly byte[] LocalFileHeader = new byte[] { 0x50, 0x4b, 0x03, 0x04 };

        /// <summary>
        /// The central directory header in zip.
        /// </summary>
        public static readonly byte[] CentralDirectoryHeader = new byte[] { 0x50, 0x4b, 0x01, 0x02 };

        /// <summary>
        /// Signature data for central directory header
        /// </summary>
        public static readonly byte[] SignatureCentralDirectory = new byte[] { 0x50, 0x4b, 0x05, 0x05 };

        /// <summary>
        /// The archive extra data record in zip.
        /// </summary>
        public static readonly byte[] ArchiveExtralDataRecord = new byte[] { 0x50, 0x4b, 0x06, 0x08};

        /// <summary>
        /// The zip64 end of central directory record
        /// </summary>
        public static readonly byte[] ZIP64EndOfCentralDirectoryRecord = new byte[] { 0x50, 0x4b, 0x06, 0x06 };

        /// <summary>
        /// The zip64 end of central directory locator
        /// </summary>
        public static readonly byte[] ZIP64EndOfCentralDirectoryLocator = new byte[] { 0x50, 0x4b, 0x06, 0x07 };

        /// <summary>
        /// End of central directory locator
        /// </summary>
        public static readonly byte[] EndOfCentralDirectoryLocator = new byte[] { 0x50, 0x4b, 0x05, 0x06 };

        /// <summary>
        /// The value 0x08074b50 has commonly been adopted as a signature value for the data descriptor record.
        /// </summary>
        public static readonly byte[] SignatureDataDescriptor = new byte[] { 0x50, 0x4b, 0x07, 0x08};

        /// <summary>
        /// End of central directory locator
        /// </summary>
        public static readonly byte[] PNG = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };


        /// <summary>
        /// Get EditorsTable from response xml.
        /// </summary>
        /// <param name="responseXml">The response xml about EditorsTable.</param>
        /// <returns>The instance of EditorsTable.</returns>
        public static EditorsTable GetEditorsTable(string responseXml)
        {
            responseXml = System.Text.RegularExpressions.Regex.Replace(responseXml, "^[^<]", string.Empty);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responseXml);
            XmlNodeList nodeList = doc.GetElementsByTagName("Editor");
            List<Editor> list = new List<Editor>();
            if (nodeList.Count > 0)
            {
                foreach (XmlNode node in nodeList)
                {
                    list.Add(GetEditor(node));
                }
            }

            EditorsTable table = new EditorsTable();
            table.Editors = list.ToArray();

            return table;
        }

        /// <summary>
        /// Get Editor instance from XmlNode.
        /// </summary>
        /// <param name="node">The XmlNode which contents the Editor data.</param>
        /// <returns>Then instance of Editor.</returns>
        private static Editor GetEditor(XmlNode node)
        {
            if (node.ChildNodes.Count == 0)
            {
                return null;
            }

            Editor editors = new Editor();
            foreach (XmlNode item in node.ChildNodes)
            {
                object propValue;
                if (item.Name == "HasEditorPermission")
                {
                    propValue = Convert.ToBoolean(item.InnerText);
                }
                else if (item.Name == "Timeout")
                {
                    propValue = Convert.ToInt64(item.InnerText);
                }
                else if (item.Name == "Metadata")
                {
                    Dictionary<string, string> metaData = new Dictionary<string, string>();
                    foreach (XmlNode metaNode in item.ChildNodes)
                    {
                        metaData.Add(metaNode.Name, System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(metaNode.InnerText)));
                    }

                    propValue = metaData;
                }
                else
                {
                    propValue = item.InnerText;
                }

                SetSpecifiedProtyValueByName(editors, item.Name, propValue);
            }

            return editors;
        }

        /// <summary>
        /// Set a value in the target object using the specified property name
        /// </summary>
        /// <param name="target">The target object</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="value">The property value</param>
        public static void SetSpecifiedProtyValueByName(object target, string propertyName, object value)
        {
            if (string.IsNullOrEmpty(propertyName) || null == value || null == target)
            {
                return;
            }

            PropertyInfo matchedProperty = GetSpecifiedPropertyByName(target, propertyName);
            if (matchedProperty != null)
            {
                matchedProperty.SetValue(target, value, null);
            }
            else
            {
                throw new InvalidOperationException("Cannot find the property name in the target type " + target.GetType().Name);
            }
        }

        /// <summary>
        /// Get a value in the target object using the specified property name
        /// </summary>
        /// <param name="target">The target object</param>
        /// <param name="propertyName">The property name value</param>
        /// <returns>The property value</returns>
        public static object GetSpecifiedPropertyValueByName(object target, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || null == target)
            {
                return null;
            }

            PropertyInfo matchedProperty = GetSpecifiedPropertyByName(target, propertyName);
            object value = null;
            if (matchedProperty != null)
            {
                value = matchedProperty.GetValue(target, null);
            }

            return value;
        }

        /// <summary>
        /// Get a value in the target object using the specified property name
        /// </summary>
        /// <param name="target">The target object</param>
        /// <param name="propertyName">The property name value</param>
        /// <returns>The property value</returns>
        public static PropertyInfo GetSpecifiedPropertyByName(object target, string propertyName)
        {
            Type currentType = target.GetType();
            PropertyInfo property = currentType.GetProperty(propertyName);
            return property;
        }
    }
}