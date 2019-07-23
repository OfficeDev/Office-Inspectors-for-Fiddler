using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml;
using System.Drawing;


namespace FSSHTTPandWOPIInspector.Parsers
{
    public abstract class BaseStructure
    {
        /// <summary>
        /// The stream to parse
        /// </summary>
        private Stream stream;

        /// <summary>
        /// editTableQueue for map with hexview
        /// </summary>
        public static Queue<int> editTableQueue = new Queue<int>();

        /// <summary>
        /// Parse stream to specific message
        /// </summary>
        /// <param name="s">Stream to parse</param>
        public virtual void Parse(Stream s)
        {
            stream = s;
        }

        /// <summary>
        /// Parse the ObjectData structure for ONESTORE message.
        /// </summary>
        /// <param name="s">A stream containing ObjectData structure.</param> 
        /// <param name="partitionId">A compact unsigned 64-bit integer that specifies the object partition Id of the object.</param> 
        public virtual void Parse(Stream s, ulong partitionId)
        {
            stream = s;
        }

        /// <summary>
        /// Parse the Data structure for ONESTORE message.
        /// </summary>
        /// <param name="s">A stream containing Data structure for ONESTORE message.</param>       
        /// <param name="is2ndParse">A bool value that specifies is 2nd Parse  Data structure for ONESTORE message.</param> 
        public virtual void Parse(Stream s, bool is2ndParse)
        {
            stream = s;
        }
   
        /// <summary>
        /// Override the ToString method to return empty.
        /// </summary>
        /// <returns>Empty string value</returns>
        public override string ToString()
        {
            return "";
        }

        /// <summary>
        /// Read  bits value from byte
        /// </summary>
        /// <param name="b">The byte.</param>
        /// <param name="index">The bit index to read</param>
        /// <param name="length">The bit length to read</param>
        /// <returns>bits value</returns>
        public byte GetBits(byte b, int index, int length)
        {
            int Bit = 0;
            int tempBit = 0;
            if ((index >= 8) || (length > 8))
            {
                throw new Exception("The range for index or length should be 0~7.");
            }

            for (int i = length - 1; i >= 0; i--)
            {
                tempBit = ((b & (1 << (index + i))) > 0) ? 1 : 0;
                Bit = (Bit << 1) | tempBit;
            }
            return (byte)Bit;
        }

        /// <summary>
        /// Read bits value from byte
        /// </summary>
        /// <param name="b">The short value used to get bit.</param>
        /// <param name="index">The bit index to read</param>
        /// <param name="length">The bit length to read</param>
        /// <returns>bits value</returns>
        public ushort GetBits(short b, int index, int length)
        {
            byte[] retBytes = new byte[16];
            if ((index >= 16) || (length > 16))
            {
                throw new Exception("The range for index or length should be 0~15.");
            }

            for (int i = 0; i < length; i++)
            {
                int tempBit = ((b & (1 << (index + i))) > 0) ? 1 : 0;
                retBytes[i] = (byte)tempBit;
            }
            return checked((ushort)ConvertFromBytes(retBytes, 16));
        }

        /// <summary>
        /// Read bits value from byte
        /// </summary>
        /// <param name="b">The int value used to get bit.</param>
        /// <param name="index">The bit index to read</param>
        /// <param name="length">The bit length to read</param>
        /// <returns>bits value</returns>
        public uint GetBits(int b, int index, int length)
        {
            byte[] retBytes = new byte[32];
            if ((index >= 32) || (length > 32))
            {
                throw new Exception("The range for index or length should be 0~32.");
            }

            for (int i = 0; i < length; i++)
            {
                int tempBit = ((b & (1 << (index + i))) > 0) ? 1 : 0;
                retBytes[i] = (byte)tempBit;
            }
            return checked((uint)ConvertFromBytes(retBytes, 32));
        }

        /// <summary>
        /// Read bits value from byte
        /// </summary>
        /// <param name="b">The long value used to get bit.</param>
        /// <param name="index">The bit index to read</param>
        /// <param name="length">The bit length to read</param>
        /// <returns>bits value</returns>
        public ulong GetBits(long b, int index, int length)
        {
            byte[] retBytes = new byte[64];
            if ((index >= 64) || (length > 64))
            {
                throw new Exception("The range for index or length should be 0~63.");
            }

            for (int i = 0; i < length; i++)
            {
                int tempBit = ((b & (1 << (index + i))) > 0) ? 1 : 0;
                retBytes[i] = (byte)tempBit;
            }

            return checked((ulong)ConvertFromBytes(retBytes, 64));
        }

        /// <summary>
        /// Returns a value built from the specified number of bytes from the given buffer
        /// </summary>
        /// <param name="buffer">Specify the data in byte array format</param>
        /// <param name="bytesToConvert">Specify the number of bytes to use</param>
        /// <returns>Return the value built from the given bytes</returns>
        private static ulong ConvertFromBytes(byte[] buffer, int bytesToConvert)
        {
            ulong ret = 0;
            int bitCount = 0;
            for (int i = 0; i < bytesToConvert; i++)
            {
                ret |= (ulong)buffer[i] << bitCount;

                bitCount += 1;
            }

            return ret;
        }

        /// <summary>
        /// Read an Int16 value from stream
        /// </summary>
        /// <returns>An Int16 value</returns>
        protected Int16 ReadINT16()
        {
            int value;
            int b1, b2;
            b1 = ReadByte();
            b2 = ReadByte();

            value = (b2 << 8) | b1;

            return (Int16)value;
        }

        /// <summary>
        /// Read an Int32 value from stream
        /// </summary>
        /// <returns>An Int32 value</returns>
        protected Int32 ReadINT32()
        {
            long value;
            int b1, b2, b3, b4;
            b1 = ReadByte();
            b2 = ReadByte();
            b3 = ReadByte();
            b4 = ReadByte();

            value = (b4 << 24) | (b3 << 16) | (b2 << 8) | b1;

            return (Int32)value;
        }

        /// <summary>
        /// Read an long value from stream
        /// </summary>
        /// <returns>An long value</returns>
        public long ReadINT64()
        {
            long low = this.ReadINT32();
            long high = this.ReadINT32();

            // 0x100000000 is 2 raised to the 32th power plus 1
            return (long)((high << 32) | low);
        }

        /// <summary>
        /// Read an Boolean value from stream
        /// </summary>
        /// <returns>An Boolean value</returns>
        protected Boolean ReadBoolean()
        {
            return ReadByte() != 0x00;
        }

        /// <summary>
        /// Read a byte value from stream
        /// </summary>
        /// <returns>A byte</returns>
        protected byte ReadByte()
        {
            int value = stream.ReadByte();
            if (value == -1)
            {
                throw new Exception();
            }
            return (byte)value;
        }

        /// <summary>
        /// Read a GUID value from stream
        /// </summary>
        /// <returns>A GUID value</returns>
        protected Guid ReadGuid()
        {
            Guid guid = new Guid(ReadBytes(16));
            if (guid == null)
            {
                throw new Exception();
            }
            return guid;
        }

        /// <summary>
        /// Read an ushort value from stream
        /// </summary>
        /// <returns>An ushort value</returns>
        protected ushort ReadUshort()
        {
            int value;
            int b1, b2;
            b1 = ReadByte();
            b2 = ReadByte();

            value = (b2 << 8) | b1;

            return (ushort)value;
        }

        /// <summary>
        /// Read an uint value from stream
        /// </summary>
        /// <returns>An uint value</returns>
        protected uint ReadUint()
        {
            long value;
            int b1, b2, b3, b4;
            b1 = ReadByte();
            b2 = ReadByte();
            b3 = ReadByte();
            b4 = ReadByte();

            value = (b4 << 24) | (b3 << 16) | (b2 << 8) | b1;

            return (uint)value;
        }

        /// <summary>
        /// Read an uLong value from stream
        /// </summary>
        /// <returns>An uLong value</returns>
        protected ulong ReadUlong()
        {
            long low = (uint)this.ReadUint();
            long high = (uint)this.ReadUint();

            return (ulong)(high << 32 | low);
        }

        /// <summary>
        /// Read an three bytes value as uint from stream
        /// </summary>
        /// <returns>An int value</returns>
        protected int Read3Bytes()
        {
            long value;
            int b1, b2, b3;
            b1 = ReadByte();
            b2 = ReadByte();
            b3 = ReadByte();

            value = (b3 << 16) | (b2 << 8) | b1;

            return (int)value;
        }

        /// <summary>
        /// Read an five bytes value as ulong from stream
        /// </summary>
        /// <returns>An long value</returns>
        protected long Read5Bytes()
        {
            long value;
            int b1, b2, b3, b4, b5;
            b1 = ReadByte();
            b2 = ReadByte();
            b3 = ReadByte();
            b4 = ReadByte();
            b5 = ReadByte();

            value = (b5 << 32) | (b4 << 24) | (b3 << 16) | (b2 << 8) | b1;
            return (long)value;
        }

        /// <summary>
        /// Read an six bytes value as ulong from stream
        /// </summary>
        /// <returns>An Long value</returns>
        protected long Read6Bytes()
        {
            long low = (int)this.Read3Bytes();
            long high = (int)this.Read3Bytes();

            return high << 24 | low;
        }

        /// <summary>
        /// Read an seven bytes value as ulong from stream
        /// </summary>
        /// <returns>An Long value</returns>
        protected long Read7Bytes()
        {
            long b1 = (int)this.Read3Bytes();
            long b2 = (int)this.ReadINT32();

            return b2 << 32 | b1;
        }

        /// <summary>
        /// Read string value from stream according to string terminator and Encoding method
        /// </summary>
        /// <param name="encoding">The character Encoding</param>
        /// <param name="terminator">The string terminator</param>
        /// <param name="length">The string length.</param>
        /// <param name="reducedUnicode">True means reduced Unicode character string. The terminating null character is one zero byte.</param>
        /// <returns>A string value</returns>
        protected string ReadString(Encoding encoding, string terminator = "\0", int stringlength = 0, bool reducedUnicode = false)
        {
            string result = null;
            StringBuilder value = new StringBuilder();
            if (stringlength == 0)
            {
                int length = terminator.Length;
                bool terminated = false;
                // Read Null-terminated reduced Unicode character string. The terminating null character is one zero byte.
                if ((encoding == Encoding.Unicode) && (reducedUnicode))
                {
                    while (!terminated)
                    {
                        byte[] tempbytes = new byte[2];
                        tempbytes[0] = ReadByte();
                        if (Encoding.ASCII.GetChars(tempbytes, 0, 1)[0].ToString() == "\0")
                        {
                            terminated = true;
                            break;
                        }
                        tempbytes[1] = ReadByte();
                        char[] chars = Encoding.Unicode.GetChars(tempbytes, 0, 2);
                        value.Append(chars);
                    }
                    result = value.ToString();
                }
                else
                {
                    while (!terminated)
                    {
                        value.Append(ReadChar(encoding));
                        if (value.Length < length)
                        {
                            continue;
                        }
                        int i;
                        for (i = length - 1; i >= 0; i--)
                        {
                            if (terminator[i] != value[value.Length - length + i])
                            {
                                break;
                            }
                        }
                        terminated = i < 0;
                    }
                    result = value.Remove(value.Length - length, length).ToString();
                }
            }
            else
            {
                int size = stringlength;
                while (size != 0)
                {
                    value.Append(ReadChar(encoding));
                    size--;
                }
                result = value.ToString();
            }
            return result;
        }

        /// <summary>
        /// Read bytes from stream
        /// </summary>
        /// <param name="length">The byte length to read</param>
        /// <returns>Bytes value</returns>
        protected byte[] ReadBytes(int length)
        {
            byte[] bytes = new byte[length];

            int count = stream.Read(bytes, 0, length);

            if (count != length)
            {
                throw new Exception();
            }
            return bytes;
        }

        /// <summary>
        /// Read character from stream
        /// </summary>
        /// <param name="encoding">The text encoding</param>
        /// <returns>A char value</returns>
        protected char ReadChar(Encoding encoding)
        {
            int length = encoding.GetMaxByteCount(1);
            byte[] bytes = new byte[length];
            int count = stream.Read(bytes, 0, length);
            if (count == -1)
            {
                throw new Exception();
            }
            char[] chars = encoding.GetChars(bytes, 0, count);

            length = encoding.GetByteCount(chars, 0, 1);
            if (length < count)
            {
                stream.Seek(length - count, SeekOrigin.Current);
            }
            return chars[0];
        }

        /// <summary>
        /// Get the current byte of stream
        /// </summary>
        /// <returns>the current byte of stream</returns>
        protected byte CurrentByte()
        {
            byte current = this.ReadByte();
            stream.Position -= 1;

            return current;
        }

        /// <summary>
        /// Get the next four bytes of stream
        /// </summary>
        /// <returns>the next four bytes of stream</returns>
        protected byte[] NextFourBytes()
        {
            byte[] fourBytes = this.ReadBytes(4);
            stream.Position -= 4;

            return fourBytes;
        }

        /// <summary>
        /// Remove subNode which node text contains "specified"
        /// </summary>
        /// <param name="treenode">The tree node used to remove sub node</param>
        /// <returns>The new treeNode has removed subnode contains "specified"</returns>
        public static TreeNode RemoveAnySpecifiedTreeNode(TreeNode treenode)
        {
            TreeNode newNode = treenode;
            List<string> nodeNames = new List<string>();
            List<int> nodeIndexs = new List<int>();
            foreach (TreeNode node in newNode.Nodes)
            {
                if (node == null)
                    break;

                if (node.Text.Contains("Specified") && node.Text.Contains("True"))
                {
                    int index = newNode.Nodes.IndexOf(node);
                    nodeIndexs.Add(index);
                    continue;
                }
                else if (node.Text.Contains("Specified") && node.Text.Contains("False"))
                {
                    int index = newNode.Nodes.IndexOf(node);
                    nodeIndexs.Add(index);
                    nodeNames.Add(node.Text);
                    continue;
                }

                if (node.Nodes.Count != 0)
                {
                    int index = newNode.Nodes.IndexOf(node);
                    newNode.Nodes[index] = RemoveAnySpecifiedTreeNode(node);
                }
            }

            // Remove the nodes contains "specified"
            if (nodeIndexs != null)
            {
                int[] nodeIndexList = nodeIndexs.ToArray();
                for (int i = 0; i < nodeIndexList.Length; i++)
                {
                    newNode.Nodes[nodeIndexList[i]].Remove();

                    for (int j = i + 1; j < nodeIndexList.Length; j++)
                        nodeIndexList[j]--;
                }
            }

            // Remove the nodes with bool defalut value
            if (nodeNames != null)
            {
                string[] nodeNamesList = nodeNames.ToArray();
                for (int i = 0; i < nodeNames.Count; i++)
                {
                    RemoveDefaultValueTreeNode(ref newNode, nodeNamesList[i]);
                }
            }

            return newNode;
        }

        /// <summary>
        /// Add a serial number only for treenodes which are FSSHTTPB structures
        /// </summary>
        /// <param name="treenode">treenode which used to add serial number</param>
        /// <param name="index">the serial number</param>
        /// <returns>The treenode whoes subnodes belongs FSSHTTPB structure with a serial number</returns>
        public static TreeNode AddserialNumForFSSHTTPBTreeNode(TreeNode treenode, ref int index)
        {
            TreeNode newNode = treenode;
            if (newNode == null)
                return null;

            for (int i = 0; i < newNode.Nodes.Count; i++)
            {
                if (newNode.Nodes[i].Text.Contains("TextObject") || newNode.Nodes[i].Text.Contains("IncludeObject"))
                {
                    newNode.Nodes[i] = AddNumberForTreeNode(newNode.Nodes[i], index);
                    index++;
                    continue;
                }

                if (newNode.Nodes[i].Nodes.Count != 0)
                {
                    newNode.Nodes[i] = AddserialNumForFSSHTTPBTreeNode(newNode.Nodes[i], ref index);
                }
            }
            return newNode;
        }

        /// <summary>
        /// Add a number attribute for treenode
        /// </summary>
        /// <param name="treenode">treenode which used to add number</param>
        /// <param name="index">the number</param>
        /// <returns>A TreeNode with a number attrubute</returns>
        public static TreeNode AddNumberForTreeNode(TreeNode treenode, int index)
        {
            TreeNode newNode = treenode;
            ((Position)newNode.Tag).Num = index;
            for (int i = 0; i < newNode.Nodes.Count; i++)
            {
                if (newNode.Nodes[i].Nodes.Count != 0)
                {
                    newNode.Nodes[i] = AddNumberForTreeNode(newNode.Nodes[i], index);
                }
                else
                {
                    ((Position)newNode.Nodes[i].Tag).Num = index;
                }
            }

            return newNode;
        }

        /// <summary>
        /// Remove subNode which contains defaut value(for optinal element)
        /// </summary>
        /// <param name="node">The tree node used to remove sub node</param>
        /// <param name="specifiedText">the specified text</param>
        /// <returns>The TreeNode with object value information</returns>
        public static void RemoveDefaultValueTreeNode(ref TreeNode node, string specifiedText)
        {
            Regex reg = new Regex(@".*(?=Specified)");
            string NodeWithDefaultValue = reg.Match(specifiedText).Value + ":";
            foreach (TreeNode n in node.Nodes)
            {
                if (n == null)
                    break;
                if (n.Text.Contains(NodeWithDefaultValue))
                {
                    n.Remove();
                    break;
                }
            }
        }

        /// <summary>
        /// Convert object to TreeNode
        /// </summary>
        /// <param name="obj">The object need to convet</param>
        /// <param name="ropNameforBinaryStrucutre">The object need to convet</param>
        /// <returns>The TreeNode with object value information</returns>
        public static TreeNode ObjectToTreeNode(object obj, string ropNameforBinaryStrucutre)
        {
            Type t = obj.GetType();
            TreeNode res = new TreeNode(t.Name);

            // Check whether the data type is simple type
            if (Enum.IsDefined(typeof(DataType), t.Name))
            {
                throw new Exception("The method doesn't support handling simple data type.");
            }
            else if (obj is Byte[])
            {
                res = ArrayObjectToNode(obj, ropNameforBinaryStrucutre);
            }
            else
            {
                // For no simple type, loop each field and convert to treenode
                PropertyInfo[] info = t.GetProperties();
                for (int i = 0; i < info.Length; i++)
                {
                    object feildValue = info[i].GetValue(obj, null);
                    if (feildValue != null)
                    {
                        Type feildType = info[i].PropertyType;
                        // For simple type and enum: 
                        if (Enum.IsDefined(typeof(DataType), feildType.Name) || (feildType.IsEnum && Enum.IsDefined(typeof(DataType), feildType.GetEnumUnderlyingType().Name)))
                        {
                            TreeNode node = SimpleTypeObjectToNode(feildValue, info[i].Name);
                            res.Nodes.Add(node);
                        }
                        else if (feildType.IsArray)
                        {
                            if (((Array)feildValue).Length > 0)
                            {
                                TreeNode node = ArrayObjectToNode(feildValue, info[i].Name);
                                res.Nodes.Add(node);
                            }
                        }
                        else // complex type
                        {
                            if (info[i].Name == "TextObject" || info[i].Name == "IncludeObject")
                            {
                                int offset = 0;
                                int startIndex = 0;
                                TreeNode node = ObjectToTreeNode(feildValue, ref startIndex, out offset);
                                node.Text = info[i].Name;
                                res.Nodes.Add(node);
                            }
                            else
                            {
                                TreeNode node = ObjectToTreeNode(feildValue, "");
                                node.Text = info[i].Name;
                                res.Nodes.Add(node);
                            }
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Convert simple type object to TreeNode
        /// </summary>
        /// <param name="obj">The object which need to convert</param>
        /// <param name="name">The name of the child node</param>
        /// <returns>The TreeNode with object value information</returns>
        public static TreeNode SimpleTypeObjectToNode(object obj, string name)
        {
            TreeNode tn = new TreeNode(string.Format("{0}:{1}", name, obj.ToString()));
            return tn;
        }

        /// <summary>
        /// Convert object array to TreeNode
        /// </summary>
        /// <param name="obj">The object which need to convert</param>
        /// <param name="name">The name of the node</param>
        /// <returns>The TreeNode with object value information</returns>
        public static TreeNode ArrayObjectToNode(object obj, string name)
        {
            Array arrayObj = (Array)obj;
            Type elementType = obj.GetType().GetElementType();
            int n = 0;
            TreeNode node = new TreeNode();
            TreeNode subnode = new TreeNode();

            if (elementType.Name == "XmlElement")
            {
                foreach (var ele in arrayObj)
                {
                    TreeNode incElement = new TreeNode();
                    incElement.Text = string.Format("{0}:{1}", ((XmlElement)ele).Name, ((XmlElement)ele).OuterXml);
                    subnode.Nodes.Add(incElement);
                    // Update Node name from feild Type to feild name.
                    subnode.Text = string.Format("{0}[{1}]", name, n++);
                    node.Nodes.Add(subnode);
                }

            }
            else if (elementType.Name == "XmlAttribute")
            {
                foreach (var ele in arrayObj)
                {
                    TreeNode Attribute = new TreeNode();
                    Attribute.Text = string.Format("{0}:{1}", ((XmlAttribute)ele).Name, ((XmlAttribute)ele).Value);
                    subnode.Nodes.Add(Attribute);
                    // Update Node name from feild Type to feild name.
                    subnode.Text = string.Format("{0}[{1}]", name, n++);
                    node.Nodes.Add(subnode);
                }
            }
            else
            {
                foreach (var ele in arrayObj)
                {
                    if (Enum.IsDefined(typeof(DataType), elementType.Name))
                    {
                        subnode = SimpleTypeObjectToNode(ele, string.Format("{0}[{1}]", name, n++));
                    }
                    else
                    {
                        subnode = ObjectToTreeNode(ele, "");
                        // Update Node name from feild Type to feild name.
                        subnode.Text = string.Format("{0}[{1}]", name, n++);
                    }
                    node.Nodes.Add(subnode);
                }
            }
            node.Text = string.Format("{0}", name);
            return node;
        }

        /// <summary>
        /// Add the object to TreeNode and calculate the byte number it consumed
        /// </summary>
        /// <param name="obj">The object need to display in TreeView</param>
        /// <param name="startIndex">The start position of the object in HexView</param>
        /// <param name="offset">The byte number consumed by the object</param>
        /// <returns>The TreeNode with object value information</returns>
        public static TreeNode ObjectToTreeNode(object obj, ref int startIndex, out int offset)
        {
            Type t = obj.GetType();
            TreeNode res = new TreeNode(t.Name);
            int objStartIndex = startIndex;
            int os = 0;
            int objos = 0;

            // Check whether the data type is simple type
            if (Enum.IsDefined(typeof(DataType), t.Name))
            {
                throw new Exception("The method doesn't support handling simple data type.");
            }
            else
            {
                // For no simple type, loop each field and convert to treenode
                FieldInfo[] info = t.GetFields();

                if (t.FullName == "FSSHTTPandWOPIInspector.Parsers.EditorsTable")
                {
                    PropertyInfo[] EditorsTable = t.GetProperties();
                    object Editors = EditorsTable[0].GetValue(obj, null);
                    Array EditorsArray = (Array)Editors;

                    if (EditorsArray.Length > 0)
                    {
                        int i = 0;
                        foreach (var eidtor in EditorsArray)
                        {
                            TreeNode node = new TreeNode(String.Format("Editors[{0}]", i));
                            res.Nodes.Add(node);
                            node.Tag = new Position(objStartIndex, objos);

                            PropertyInfo[] Editorpro = eidtor.GetType().GetProperties();
                            for (int j = 0; j < Editorpro.Length; j++)
                            {
                                if (Editorpro[j].GetValue(eidtor, null) != null)
                                {
                                    if (Editorpro[j].Name == "Metadata")
                                    {
                                        TreeNode subNode = new TreeNode(string.Format("{0}", Editorpro[j].Name));
                                        node.Nodes.Add(subNode);
                                        subNode.Tag = new Position(objStartIndex, objos);
                                        Dictionary<string, string> MetaData = (Dictionary<string, string>)Editorpro[j].GetValue(eidtor, null);
                                        foreach (var item in MetaData)
                                        {
                                            TreeNode leftNode = new TreeNode(string.Format("{0}:{1}", item.Key, item.Value));
                                            subNode.Nodes.Add(leftNode);
                                            leftNode.Tag = new Position(objStartIndex, objos);
                                        }

                                    }
                                    else
                                    {
                                        TreeNode subNode = new TreeNode(string.Format("{0}:{1}", Editorpro[j].Name, Editorpro[j].GetValue(eidtor, null)));
                                        node.Nodes.Add(subNode);
                                        subNode.Tag = new Position(objStartIndex, objos);
                                    }
                                }
                            }
                            i++;
                        }
                    }

                    objos = editTableQueue.Dequeue();
                    startIndex += objos;
                }
                else
                {
                    for (int i = 0; i < info.Length; i++)
                    {
                        object feildValue = info[i].GetValue(obj);

                        if (feildValue != null)
                        {
                            Type feildType = info[i].FieldType;

                            // If the field type is nullable simple data type (such as byte?), set the field data type as basic data type (such as byte in byte?)
                            if (feildType.IsGenericType && feildType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                feildType = feildType.GetGenericArguments()[0];
                            }

                            // For simple type and enum: 
                            if (Enum.IsDefined(typeof(DataType), feildType.Name) || (feildType.IsEnum && Enum.IsDefined(typeof(DataType), feildType.GetEnumUnderlyingType().Name)))
                            {
                                int current = startIndex;
                                TreeNode node = SimpleTypeObjectToNode(feildValue, obj, info[i], ref startIndex, out os);
                                res.Nodes.Add(node);
                            }
                            else if (feildType.IsArray || (feildType is System.Object && feildValue.GetType().Name == "Byte[]"))// the second condition is for object parser as byte[].
                            {
                                if (((Array)feildValue).Length > 0)
                                {
                                    int current = startIndex;
                                    TreeNode node = ArrayObjectToNode(feildValue, obj, info[i], ref startIndex, out os);
                                    res.Nodes.Add(node);
                                }
                            }
                            else // complex type
                            {
                                int current = startIndex;
                                TreeNode node = ObjectToTreeNode(feildValue, ref startIndex, out os);

                                node.Text = info[i].Name;
                                res.Nodes.Add(node);
                            }
                        }
                    }
                    objos = startIndex - objStartIndex; // here startIndex is the next field's start index,
                }
            }


            Position objps = new Position(objStartIndex, objos);
            res.Tag = objps;
            offset = objos;
            return res;
        }

        /// <summary>
        /// Convert simple type object to TreeNode
        /// </summary>
        /// <param name="obj">The object which need to convert</param>
        /// <param name="parentobj">The parent object of the object which need to convert</param>
        /// <param name="Info">The field Info of the object in parent object</param>
        /// <param name="startIndex">The start index of this object in message stream</param>
        /// <param name="offset">The offset of this object</param>
        /// <returns>The TreeNode with object value information</returns>
        public static TreeNode SimpleTypeObjectToNode(object obj, object parentobj, FieldInfo Info, ref int startIndex, out int offset)
        {
            int os = 0;
            TreeNode tn = new TreeNode(string.Format("{0}:{1}", Info.Name, obj.ToString()));
            Type type = obj.GetType();
            if (Info.GetCustomAttributesData().Count != 0 && Info.GetCustomAttributes(typeof(BitAttribute), false) != null)
            {
                Type parentType = parentobj.GetType();
                FieldInfo[] FieldsInparent = parentType.GetFields();
                List<int> FieldsWithBitAttribute = new List<int>();
                foreach (FieldInfo f in FieldsInparent)
                {
                    if (f.GetCustomAttributesData().Count != 0 && f.GetCustomAttributes(typeof(BitAttribute), false) != null)
                    {
                        BitAttribute attribute = (BitAttribute)f.GetCustomAttributes(typeof(BitAttribute), false)[0];
                        FieldsWithBitAttribute.Add(attribute.BitLength);

                        if (f == Info)
                            break;
                    }
                }
                int FieldsBitLength = FieldsWithBitAttribute.Sum();
                int[] Fields = FieldsWithBitAttribute.ToArray();

                int n = FieldsBitLength / 8;
                int r = FieldsBitLength % 8;
                int thisFieldBitLength = Fields[Fields.Length - 1];
                int thisn = thisFieldBitLength / 8;
                int forntEleSum = FieldsBitLength - thisFieldBitLength; // This value indicate the fields sum before this field.
                for (int i = FieldsWithBitAttribute.Count - 2; i > 0; i--) // This loop is used to update n and r, if there are fields sum is xompleted bytes, so n and r should subtract the byte length. 
                {
                    forntEleSum -= Fields[i];
                    if (forntEleSum % 8 == 0)
                    {
                        n = (FieldsBitLength - forntEleSum) / 8;
                        r = (FieldsBitLength - forntEleSum) % 8;
                        break;
                    }
                }
                if (FieldsWithBitAttribute.Count != 1 && (FieldsBitLength - thisFieldBitLength) % 8 != 0)
                {
                    startIndex -= 1;
                }
                if (thisn < n)
                {
                    if ((FieldsBitLength - thisFieldBitLength) % 8 == 0)// This is for bit field which is start from a new byte, but is not the first bit filed in parent field.
                    {
                        os += (thisn + 1);
                    }
                    else
                    {
                        if (r != 0)
                            os += (thisn + 2);
                        else
                            os += (thisn + 1);
                    }
                }
                else
                {
                    os += (thisn + 1);
                }
            }
            else
            {
                if (type.IsEnum)
                {
                    Type t = type.GetEnumUnderlyingType();
                    os = Marshal.SizeOf(t);
                }
                else if (type.Name == "String")
                {
                    if (Info.Name == "FileName")//For ZIP structure in FSSHTTPD 
                    {
                        os = ((string)obj).Length;
                    }
                    else
                    {
                        os = ((string)obj).Length * 2;
                    }
                }
                else if (type.Name != "Boolean")
                {
                    os = Marshal.SizeOf(type);
                }
                else
                {
                    os = sizeof(Boolean);
                }
            }

            Position ps = new Position(startIndex, os);
            tn.Tag = ps;
            startIndex += os;
            offset = os;
            return tn;
        }

        /// <summary>
        /// Convert object array to TreeNode
        /// </summary>
        /// <param name="obj">The object which need to convert</param>
        /// <param name="parentobj">The parent object of the object which need to convert</param>
        /// <param name="Info">The field Info of the object in parent object</param>
        /// <param name="startIndex">The start index of this object in message stream</param>
        /// <param name="offset">The offset of this object</param>
        /// <returns>The TreeNode with object value information</returns>
        public static TreeNode ArrayObjectToNode(object obj, object parentobj, FieldInfo Info, ref int startIndex, out int offset)
        {
            Array arrayObj = (Array)obj;
            Type elementType = obj.GetType().GetElementType();
            int n = 0;
            TreeNode node = new TreeNode();
            TreeNode subnode = new TreeNode();
            int os = 0;
            int Arrayos = 0;
            int ArrayStart = startIndex;
            if (arrayObj.Length >= 100)
            {
                node.Text = string.Format("{0}", Info.Name + "...");
                Arrayos = arrayObj.Length;
                startIndex += Arrayos;
            }
            else
            {
                node.Text = string.Format("{0}", Info.Name);
                foreach (var ele in arrayObj)
                {
                    if (Enum.IsDefined(typeof(DataType), elementType.Name))
                    {
                        subnode = SimpleTypeObjectToNode(ele, parentobj, Info, ref startIndex, out os);
                        // Update Node name from feild Type to feild name.
                        subnode.Text = string.Format("{0}[{1}]:{2}", Info.Name, n++, ele.ToString());
                    }
                    else
                    {
                        subnode = ObjectToTreeNode(ele, ref startIndex, out os);
                        // Update Node name from feild Type to feild name.
                        subnode.Text = string.Format("{0}[{1}]", Info.Name, n++);
                    }

                    node.Nodes.Add(subnode);
                    Arrayos += os;
                }
            }


            Position ps = new Position(ArrayStart, Arrayos);
            node.Tag = ps;
            offset = Arrayos;
            return node;
        }

        #region Helper for AddNodesForTree function
        /// <summary>
        /// Record start position and byte counts consumed 
        /// </summary>
        public class Position
        {
            public int StartIndex;
            public int Offset;
            public int Num;
            public Position(int startIndex, int offset)
            {
                this.StartIndex = startIndex;
                this.Offset = offset;
            }
        }

        /// <summary>
        /// The data type enum
        /// </summary>
        public enum DataType
        {
            Binary,
            Boolean,
            Byte,
            Char,
            Double,
            Decimal,
            Single,
            Guid,
            Int16,
            Int32,
            Int64,
            SByte,
            String,
            UInt16,
            UInt32,
            UInt64,
            DateTime
        }
        #endregion

        #region Helper for FSHHTPB
        /// <summary>
        /// specify whether stream contains Stream Object Header
        /// </summary>
        /// <param name="headerType">a ushort indicate the type of the stream object header</param>
        /// <returns>bool value indicates whether the stream contains Stream object header in current postion</returns>
        public bool ContainsStreamObjectHeader(ushort headerType)
        {
            byte lsb = ReadByte();
            stream.Position -= 1;

            // StreamObjectStart16BitHeader
            if ((lsb & 0x03) == 0x0)
            {
                long header16BitValue = GetBigEnidan(2);
                return (header16BitValue >> 3 & 0x3F) == headerType;
            }

            // StreamObjectStart32BitHeader
            if ((lsb & 0x03) == 0x2)
            {
                long header32BitValue = GetBigEnidan(3);
                return (header32BitValue >> 3 & 0x3FFF) == headerType;
            }
            return false;
        }

        /// <summary>
        /// specify whether stream contains Stream Object Start 16Bit Header
        /// </summary>
        /// <param name="headerType">a ushort indicate the type of the 16 bit stream object header</param>
        /// <returns>bool value indicates whether the stream contains a 16bit Stream object header in current postion</returns>
        public bool ContainsStreamObjectStart16BitHeader(ushort headerType)
        {
            byte lsb = ReadByte();
            stream.Position -= 1;
            if ((lsb & 0x03) == 0x0)
            {
                long header16BitValue = GetBigEnidan(2);
                return (header16BitValue >> 3 & 0x3F) == headerType;
            }
            return false;
        }

        /// <summary>
        /// specify whether stream contains Stream Object Start 32Bit Header
        /// </summary>
        /// <param name="headerType">a ushort indicate the type of the 32 bit stream object header</param>
        /// <returns>bool value indicates whether the stream contains a 32bit Stream object header in current postion</returns>
        public bool ContainsStreamObjectStart32BitHeader(ushort headerType)
        {
            byte lsb = ReadByte();
            stream.Position -= 1;
            // StreamObjectStart32BitHeader
            if ((lsb & 0x03) == 0x2)
            {
                long header32BitValue = GetBigEnidan(3);
                return (header32BitValue >> 3 & 0x3FFF) == headerType;
            }

            return false;
        }

        /// <summary>
        /// read bytes from stream to long in big enidan.
        /// </summary>
        /// <param name="byteNumber">byte number need to read</param>
        /// <returns>the big endian int value</returns>
        public long GetBigEnidan(int byteNumber)
        {
            long result = 0x0;

            for (int i = 0; i < byteNumber; i++)
            {
                long byteOfi = stream.ReadByte();
                result = result | (byteOfi << 8 * i);
            }
            stream.Position -= byteNumber;
            return result;
        }

        /// <summary>
        /// Get the length of the ExtenedGUID
        /// </summary>
        /// <param name="byteOffset">the offset from the current position of the stream</param>
        /// <returns>the size of the ExtendedGUID in bytes</returns>
        public int GetExtendedGUIDBytesLen(int byteOffset)
        {
            stream.Position = stream.Position + byteOffset;
            byte lsb = ReadByte();
            stream.Position -= (byteOffset + 1);
            if (lsb == 0x0)
                return 1;

            if (lsb == 0x80)
                return 21;

            if ((lsb & 0x07) == 0x4)
                return 17;

            if ((lsb & 0x3F) == 0x20)
                return 18;

            if ((lsb & 0x7F) == 0x40)
                return 19;

            return 0;
        }

        /// <summary>
        /// Get the length of the SerialNumber
        /// </summary>
        /// <param name="byteOffset">the offset from the current position of the stream</param>
        /// <returns>the size of the SerialNumber in bytes</returns>
        public int GetSerialNumberBytesLen(int byteOffset)
        {
            stream.Position = stream.Position + byteOffset;
            byte lsb = ReadByte();
            stream.Position -= (byteOffset + 1);
            if (lsb == 0x0)
                return 1;

            if (lsb == 0x80)
                return 25;

            return 0;
        }

        /// <summary>
        /// Read CompactUnsigned64bitInteger from stream
        /// </summary>
        /// <param name="byteOffset">the offset from the current position of the stream</param>
        /// <returns></returns>
        public long ReadCompactUnsigned64bitIntegerValue(int byteOffset)
        {
            stream.Position = stream.Position + byteOffset;
            byte lsb = ReadByte();
            stream.Position -= (byteOffset + 1);
            if (lsb == 0x0)
                return lsb;

            if (lsb == 0x80)
                return GetBigEnidan(8);

            if ((lsb & 0x01) == 0x1)
                return (lsb >> 1 & 0x7F);

            if ((lsb & 0x03) == 0x2)
            {
                long rawValue = GetBigEnidan(2);
                return ((rawValue >> 2) & 0x3FFF);
            }

            if ((lsb & 0x07) == 0x4)
            {
                long rawValue = GetBigEnidan(3);
                return (rawValue >> 3 & 0x1FFFFF);
            }

            if ((lsb & 0x0F) == 0x8)
            {
                long rawValue = GetBigEnidan(4);
                return (rawValue >> 4 & 0xFFFFFFF);
            }

            if ((lsb & 0x1F) == 0x10)
            {
                long rawValue = GetBigEnidan(5);
                return (rawValue >> 5 & 0x7FFFFFFFF);
            }

            if ((lsb & 0x3F) == 0x20)
            {
                long rawValue = GetBigEnidan(6);
                return (rawValue >> 6 & 0x3FFFFFFFFFF);
            }

            if ((lsb & 0x7F) == 0x40)
            {
                long rawValue = GetBigEnidan(7);
                return (rawValue >> 7 & 0x1FFFFFFFFFFFF);
            }

            return 0;
        }

        /// <summary>
        /// Get DataElementPackage Type
        /// </summary>
        /// <returns>DataElementPackage type</returns>
        public long PreReadDataElementPackageType()
        {
            int dataElementStartLen = 2;
            int guidLen = GetExtendedGUIDBytesLen(dataElementStartLen);
            int snLen = GetSerialNumberBytesLen(guidLen + dataElementStartLen);
            return ReadCompactUnsigned64bitIntegerValue(guidLen + dataElementStartLen + snLen);
        }
        #endregion
    }

    /// <summary>
    /// Custom attribute for bit length
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BitAttribute : System.Attribute
    {
        public readonly int BitLength;
        public BitAttribute(int bitLength)
        {
            this.BitLength = bitLength;
        }
    }

    /// <summary>
    /// String encoding enum
    /// </summary>
    public enum StringEncoding
    {
        ASCII,
        Unicode
    }
}