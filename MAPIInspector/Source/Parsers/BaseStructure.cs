namespace MAPIInspector.Parsers
{
    using MapiInspector;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// String encoding enum
    /// </summary>
    public enum StringEncoding
    {
        /// <summary>
        /// ASCII encoding
        /// </summary>
        ASCII,

        /// <summary>
        /// Unicode encoding
        /// </summary>
        Unicode
    }

    /// <summary>
    /// BaseStructure class 
    /// </summary>
    public abstract class BaseStructure
    {
        /// <summary>
        /// Boolean value, if payload is compressed or obfuscated, value is true. otherwise, value is false.
        /// </summary>
        public static bool IsCompressedXOR = false;

        /// <summary>
        /// This field is for rgbOutputBuffer or ExtendedBuffer_Input in MAPIHTTP layer
        /// </summary>
        private static int compressBufferindex = 0;

        /// <summary>
        /// The stream to parse
        /// </summary>
        private Stream stream;

        /// <summary>
        /// The data type enum
        /// </summary>
        public enum DataType
        {
            /// <summary>
            /// Binary type
            /// </summary>
            Binary,

            /// <summary>
            /// Boolean type
            /// </summary>
            Boolean,

            /// <summary>
            /// Byte type
            /// </summary>
            Byte,

            /// <summary>
            /// Char type
            /// </summary>
            Char,

            /// <summary>
            /// Double type
            /// </summary>
            Double,

            /// <summary>
            /// Decimal type
            /// </summary>
            Decimal,

            /// <summary>
            /// Single type
            /// </summary>
            Single,

            /// <summary>
            /// GUID type
            /// </summary>
            Guid,

            /// <summary>
            /// Int16 type
            /// </summary>
            Int16,

            /// <summary>
            /// Int32 type
            /// </summary>
            Int32,

            /// <summary>
            /// Int64 type
            /// </summary>
            Int64,

            /// <summary>
            /// SByte type
            /// </summary>
            SByte,

            /// <summary>
            /// String type
            /// </summary>
            String,

            /// <summary>
            /// UInt16 type
            /// </summary>
            UInt16,

            /// <summary>
            /// UInt32 type
            /// </summary>
            UInt32,

            /// <summary>
            /// UInt64 type
            /// </summary>
            UInt64,

            /// <summary>
            /// DateTime type
            /// </summary>
            DateTime
        }

        /// <summary>
        /// Add the object to TreeNode and calculate the byte number it consumed
        /// </summary>
        /// <param name="obj">The object need to display in TreeView</param>
        /// <param name="startIndex">The start position of the object in HexView</param>
        /// <param name="offset">The byte number consumed by the object</param>
        /// <returns>The TreeNode with object value information</returns>
        public static TreeNode AddNodesForTree(object obj, int startIndex, out int offset)
        {
            Type t = obj.GetType();
            int current = startIndex;
            TreeNode res = new TreeNode(t.Name);

            if (t.Name == "MAPIString")
            {
                int os = 0;
                FieldInfo[] infoString = t.GetFields();
                string terminator = (string)infoString[2].GetValue(obj);
                TreeNode node = new TreeNode(string.Format("{0}:{1}", infoString[0].Name, infoString[0].GetValue(obj)));

                // If the Encoding is Unicode.
                if (infoString[1].GetValue(obj).ToString() == "System.Text.UnicodeEncoding")
                {
                    // If the StringLength is not equal 0, the StringLength will be os value.
                    if (infoString[3].GetValue(obj).ToString() != "0")
                    {
                        os = ((int)infoString[3].GetValue(obj)) * 2;
                    }
                    else
                    {
                        if (infoString[0].GetValue(obj) != null)
                        {
                            os = ((string)infoString[0].GetValue(obj)).Length * 2;
                        }

                        if (infoString[4].GetValue(obj).ToString() != "False")
                        {
                            os -= 1;
                        }

                        os += terminator.Length * 2;
                    }
                }
                else
                {
                    // If the Encoding is ASCII.
                    if (infoString[3].GetValue(obj).ToString() != "0")
                    {
                        // If the StringLength is not equal 0, the StringLength will be os value
                        os = (int)infoString[3].GetValue(obj);
                    }
                    else
                    {
                        if (infoString[0].GetValue(obj) != null)
                        {
                            os = ((string)infoString[0].GetValue(obj)).Length;
                        }

                        os += terminator.Length;
                    }
                }

                offset = os;
                Position positionString = new Position(current, os);
                node.Tag = positionString;
                res.Nodes.Add(node);
                return res;
            }
            else if (t.Name == "MAPIStringAddressBook")
            {
                FieldInfo[] infoString = t.GetFields();

                // MagicByte node
                if (infoString[1].GetValue(obj) != null)
                {
                    TreeNode nodeMagic = new TreeNode(string.Format("{0}:{1}", infoString[1].Name, infoString[1].GetValue(obj)));
                    Position positionStringMagic = new Position(current, 1);
                    nodeMagic.Tag = positionStringMagic;
                    res.Nodes.Add(nodeMagic);
                    current += 1;
                }

                // value node
                string terminator = (string)infoString[3].GetValue(obj);
                int os = 0;
                TreeNode node = new TreeNode(string.Format("{0}:{1}", infoString[0].Name, infoString[0].GetValue(obj)));

                // If the Encoding is Unicode.
                if (infoString[2].GetValue(obj).ToString() == "System.Text.UnicodeEncoding")
                {
                    // If the StringLength is not equal 0, the StringLength will be OS value.
                    if (infoString[4].GetValue(obj).ToString() != "0")
                    {
                        os = ((int)infoString[4].GetValue(obj)) * 2;
                    }
                    else
                    {
                        if (infoString[0].GetValue(obj) != null)
                        {
                            os = ((string)infoString[0].GetValue(obj)).Length * 2;
                        }

                        if (infoString[5].GetValue(obj).ToString() != "False")
                        {
                            os -= 1;
                        }

                        os += terminator.Length * 2;
                    }
                }
                else
                {
                    // If the Encoding is ASCII.
                    if (infoString[4].GetValue(obj).ToString() != "0")
                    {
                        // If the StringLength is not equal 0, the StringLength will be OS value.
                        os = (int)infoString[4].GetValue(obj);
                    }
                    else
                    {
                        if (infoString[0].GetValue(obj) != null)
                        {
                            os = ((string)infoString[0].GetValue(obj)).Length;
                        }

                        os += terminator.Length;
                    }
                }

                Position positionString = new Position(current, os);
                node.Tag = positionString;
                res.Nodes.Add(node);

                if (infoString[1].GetValue(obj) != null)
                {
                    offset = os + 1;
                }
                else
                {
                    offset = os;
                }

                return res;
            }
            else if (t.Name == "AnnotatedBytes")
            {
                var infoString = t.GetFields();
                var bytes = (byte[])infoString[0].GetValue(obj);
                var bytesString = Utilities.ConvertByteArrayToHexString(bytes);
                var annotation = (string)infoString[1].GetValue(obj);
                var node = new TreeNode($"{infoString[0].Name}:{bytesString}");

                offset = bytes.Length;
                node.Tag = new Position(current, offset);
                res.Nodes.Add(node);
                var annotationNode = new TreeNode($"annotation:{annotation}");
                annotationNode.Tag = new Position(current, offset);
                res.Nodes.Add(annotationNode);
                return res;
            }

            // Check whether the data type is simple type
            if (Enum.IsDefined(typeof(DataType), t.Name))
            {
                throw new Exception("The method doesn't support handling simple data type.");
            }
            else
            {
                // If the data type is not simple type, we will loop each field, check the data type and then parse the value in different format
                FieldInfo[] info = t.GetFields();
                int bitLength = 0;

                // The is only for FastTransfer stream parse, Polymorphic in PropValue and NamedPropInfo 
                if (obj is PropValue || obj is NamedPropInfo)
                {
                    info = MoveFirstNFieldsBehind(info, info.Length - 2);
                }

                for (int i = 0; i < info.Length; i++)
                {
                    int os = 0;
                    Type type = info[i].FieldType;

                    // If the field type is object type and its value is not null, set the field data type as details type (such as field A is Object (int/short) type, here setting it as int or short type)
                    if (type.Name == "Object" && info[i].GetValue(obj) != null)
                    {
                        type = info[i].GetValue(obj).GetType();
                    }

                    // If the field type is null-able simple data type (such as int?), set the field data type as basic data type (such as int in int?)
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GetGenericArguments()[0];
                    }

                    // Check whether the field data type is simple type: 
                    // Boolean, Byte, Char, Double, Decimal,Single, GUID, Int16, Int32, Int64, SByte, String, UInt16, UInt32, UInt64, DateTime
                    // calculate each field's offset and length.
                    if (Enum.IsDefined(typeof(DataType), type.Name))
                    {
                        if (info[i].GetValue(obj) != null)
                        {
                            Type fieldType = type;
                            TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, info[i].GetValue(obj).ToString()));
                            res.Nodes.Add(tn);

                            if (type.Name == "UInt64")
                            {
                                if (info[i].GetCustomAttributesData().Count == 0)
                                {
                                    os = 8;
                                }
                                else
                                {
                                    object[] attributes = info[i].GetCustomAttributes(typeof(BytesAttribute), false);
                                    os = (int)((BytesAttribute)attributes[0]).ByteLength;
                                }
                            }
                            else if (type.Name == "DateTime")
                            {
                                os = 8;
                            }
                            else if (type.Name == "Byte" && info[i].GetCustomAttributesData().Count != 0 && info[i].GetCustomAttributes(typeof(BitAttribute), false) != null)
                            {
                                // Check if it is bit.
                                BitAttribute attribute = (BitAttribute)info[i].GetCustomAttributes(typeof(BitAttribute), false)[0];

                                if (bitLength % 8 == 0)
                                {
                                    os += 1;
                                }
                                else
                                {
                                    current -= 1;
                                    os += 1;
                                }

                                bitLength += attribute.BitLength;
                            }
                            else if (type.Name != "Boolean")
                            {
                                os = Marshal.SizeOf(fieldType);
                            }
                            else
                            {
                                os = sizeof(bool);
                            }

                            Position ps = new Position(current, os);
                            tn.Tag = ps;
                            current += os;
                        }
                    }
                    else if ((type.IsEnum && Enum.IsDefined(typeof(DataType), type.GetEnumUnderlyingType().Name)) && info[i].GetValue(obj) != null)
                    {
                        // Else if the field data type is enum data type, its underlying type is simple type and its value is not null, calculate each field's offset
                        // and length. There are two situations: one is string type, we should calculate it's actual length (via getting value); another one is calculating
                        // the size of underlying type of enum. 
                        Type fieldType = type;

                        TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, EnumToString(info[i].GetValue(obj))));
                        res.Nodes.Add(tn);

                        if (type.Name == "String")
                        {
                            os = ((string)info[i].GetValue(obj)).Length;
                        }
                        else if (info[i].GetCustomAttributesData().Count != 0 && info[i].GetCustomAttributes(typeof(BitAttribute), false) != null)
                        {
                            // Modify the bit OS for the NotificationFlagsT in MSOXCNOTIF
                            BitAttribute attribute = (BitAttribute)info[i].GetCustomAttributes(typeof(BitAttribute), false)[0];

                            if (bitLength % 8 != 0)
                            {
                                current -= 1;
                            }

                            if (attribute.BitLength % 8 == 0)
                            {
                                os += attribute.BitLength / 8;
                            }
                            else
                            {
                                os += (attribute.BitLength / 8) + 1;
                            }

                            bitLength += attribute.BitLength;
                        }
                        else
                        {
                            os = Marshal.SizeOf(fieldType.GetEnumUnderlyingType());
                        }

                        Position ps = new Position(current, os);
                        tn.Tag = ps;
                        current += os;
                    }
                    else if (type.IsArray)
                    {
                        // If the field type is array, there are two properties need to know: optional or required, array element data type is simple or complex
                        // Field value considered: empty, null or value type displaying when not null/empty
                        // Getting the element type for required and optional array value
                        Type elementType = type.GetElementType();
                        if (!type.IsValueType && type.GetGenericArguments().Length > 0)
                        {
                            elementType = type.GetGenericArguments()[0];
                        }

                        // If the element type is simple data type, displaying the array in one line with string format.
                        if (Enum.IsDefined(typeof(DataType), elementType.Name))
                        {
                            Array arr = (Array)info[i].GetValue(obj);

                            if (arr != null && arr.Length != 0)
                            {
                                StringBuilder result = new StringBuilder();

                                // it the field is 6 bytes, updated the display text.
                                if (arr.Length == 6 && arr.GetType().ToString() == "System.Byte[]" && info[i].Name == "GlobalCounter")
                                {
                                    byte[] tempbytes = (byte[])info[i].GetValue(obj);
                                    result.Append("0x");

                                    foreach (byte tempbye in tempbytes)
                                    {
                                        result.Append(tempbye.ToString("X2"));
                                    }
                                }
                                else
                                {
                                    result.Append(Utilities.ConvertByteArrayToHexString((byte[])arr));
                                }

                                TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, result.ToString()));
                                res.Nodes.Add(tn);

                                for (int j = 0; j < arr.Length; j++)
                                {
                                    os += Marshal.SizeOf(elementType);
                                }

                                Position ps = new Position(current, os);
                                tn.Tag = ps;
                                current += os;
                            }
                        }
                        else
                        {
                            // Else if the element data type is not simple type, here will consider array type and complex type
                            Array arr = (Array)info[i].GetValue(obj);
                            object[] a = (object[])arr;

                            if (arr != null && arr.Length != 0)
                            {
                                string fieldNameForAut = info[i].Name;
                                TreeNode treeNodeArray = new TreeNode(info[i].Name);
                                TreeNode tn = new TreeNode();
                                int arros = 0;

                                if (fieldNameForAut == "RgbOutputBuffers" || fieldNameForAut == "buffers")
                                {
                                    compressBufferindex = 0;
                                }

                                for (int k = 0; k < arr.Length; k++)
                                {
                                    if (a[k] != null)
                                    {
                                        // If the item in array contains array (byte or other simple type), display the value in one line and set the offset and length.
                                        if (a[k].GetType().IsArray && a[k].GetType().GetElementType().Name == "Byte")
                                        {
                                            StringBuilder result = new StringBuilder("[");
                                            Position ps;

                                            foreach (var ar in (byte[])a[k])
                                            {
                                                result.Append(ar.ToString() + ",");
                                            }

                                            result.Remove(result.Length - 1, 1);
                                            result.Append("]");

                                            if (arr.Length == 1)
                                            {
                                                tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, result.ToString()));
                                                os = ((byte[])a[k]).Length;
                                                ps = new Position(current, os);
                                                tn.Tag = ps;
                                            }
                                            else
                                            {
                                                tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, result.ToString()));
                                                treeNodeArray.Nodes.Add(tn);
                                                os = ((byte[])a[k]).Length;
                                                ps = new Position(current, os);
                                                tn.Tag = ps;
                                            }

                                            os = ((byte[])a[k]).Length;
                                            ps = new Position(current, os);
                                            treeNodeArray.Tag = ps;
                                        }
                                        else
                                        {
                                            // If the item in array is complex type, loop call the function to add it to tree.
                                            // compressBufferindex is used to recored the rgbOutputBuffer or ExtendedBuffer_Input number here
                                            if (a.GetType().Name == "RgbOutputBuffer[]" || a.GetType().Name == "ExtendedBuffer_Input[]")
                                            {
                                                compressBufferindex += 1;
                                            }

                                            tn = AddNodesForTree(a[k], current, out os);
                                            treeNodeArray.Nodes.Add(tn);
                                            Position ps = new Position(current, os);
                                            tn.Tag = ps;
                                        }
                                    }

                                    current += os;
                                    arros += os;
                                }

                                Position pss = new Position(current - arros, arros);
                                treeNodeArray.Tag = pss;
                                res.Nodes.Add(treeNodeArray);
                            }
                        }
                    }
                    else
                    {
                        // If the field type is complex type, loop call the function until finding its data type.
                        if (info[i].GetValue(obj) != null)
                        {
                            string fieldName = info[i].Name;
                            TreeNode node = new TreeNode();

                            // The below logical is used to check whether the payload is compressed or XOR.
                            if (fieldName == "RPCHEADEREXT")
                            {
                                if (((ushort)((RPC_HEADER_EXT)info[i].GetValue(obj)).Flags & 0x0002) == (ushort)RpcHeaderFlags.XorMagic
                                   || ((ushort)((RPC_HEADER_EXT)info[i].GetValue(obj)).Flags & 0x0001) == (ushort)RpcHeaderFlags.Compressed)
                                {
                                    IsCompressedXOR = true;
                                }
                                else
                                {
                                    IsCompressedXOR = false;
                                }
                            }

                            // If the field name is Payload and its compressed, recalculating the offset and length, else directly loop call this function
                            if (fieldName == "Payload" && IsCompressedXOR)
                            {
                                RPC_HEADER_EXT header = (RPC_HEADER_EXT)info[0].GetValue(obj);
                                node = AddNodesForTree(info[i].GetValue(obj), current, out os);
                                Position nodePosition = (Position)node.Tag;
                                nodePosition.Offset = header.Size;
                                os = nodePosition.Offset;
                                node.Tag = nodePosition;
                                fieldName = "Payload(CompressedOrObfuscated)";
                                node.Text = fieldName;
                                node = TreeNodeForCompressed(node, current, compressBufferindex - 1);
                            }
                            else
                            {
                                if (fieldName == "Payload")
                                {
                                    // minus the Payload is not in compressed
                                    compressBufferindex -= 1;
                                }

                                node = AddNodesForTree(info[i].GetValue(obj), current, out os);
                                Position nodePosition = new Position(current, os);
                                node.Tag = nodePosition;
                            }

                            // Add the specific type(FastTransfer stream type) for TransferBuffer and TransferData fields.
                            if (fieldName == "TransferBuffer" || fieldName == "TransferData")
                            {
                                fieldName = string.Format(fieldName + ": " + info[i].GetValue(obj).GetType().Name);
                            }

                            node.Text = fieldName;
                            res.Nodes.Add(node);
                            current += os;
                        }
                    }
                }
            }

            offset = current - startIndex;
            Position position = new Position(startIndex, offset);
            res.Tag = position;

            return res;
        }

        /// <summary>
        /// Modify the start index for the TreeNode which source data is compressed
        /// </summary>
        /// <param name="node">The node in compressed buffers</param>
        /// <param name="current">Indicates start position of the node</param>
        /// <param name="compressBufferindex">Indicates the index of this node in all compressed buffers in same session</param>
        /// <param name="isAux">Indicates whether the buffer which this node are in is auxiliary</param>
        /// <returns>The tree node with BufferIndex and IsCompressedXOR properties </returns>
        public static TreeNode TreeNodeForCompressed(TreeNode node, int current, int compressBufferindex, bool isAux = false)
        {
            foreach (TreeNode n in node.Nodes)
            {
                TreeNode nd = n;

                if (nd.Tag != null)
                {
                    ((Position)nd.Tag).IsCompressedXOR = true;
                    ((Position)nd.Tag).StartIndex -= current;
                    ((Position)nd.Tag).BufferIndex = compressBufferindex;
                }

                if (nd.Nodes.Count != 0)
                {
                    TreeNodeForCompressed(nd, current, compressBufferindex, isAux);
                }
            }

            return node;
        }

        /// <summary>
        /// Moving the number of fields in FieldInfo from beginning to the end
        /// </summary>
        /// <param name="field">The parent field</param>
        /// <param name="n">The number of fields need moved</param>
        /// <returns>FieldInfo value field</returns>
        public static FieldInfo[] MoveFirstNFieldsBehind(FieldInfo[] field, int n)
        {
            FieldInfo[] newField = new FieldInfo[field.Length];

            if (n < 0 || n > field.Length)
            {
                throw new InvalidOperationException(string.Format("Moving Failed because the length ({0}) need to move is exceeded the fields' length ({1}).", n, field.Length));
            }
            else
            {
                int i = 0;

                for (; i < field.Length - n; i++)
                {
                    newField[i] = field[n + i];
                }

                for (; i < field.Length; i++)
                {
                    newField[i] = field[i - (field.Length - n)];
                }

                return newField;
            }
        }

        /// <summary>
        /// Parse stream to specific message
        /// </summary>
        /// <param name="s">Stream to parse</param>
        public virtual void Parse(Stream s)
        {
            this.stream = s;
        }

        /// <summary>
        /// Override the ToString method to return empty.
        /// </summary>
        /// <returns>Empty string value</returns>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <summary>
        /// Read bits value from byte
        /// </summary>
        /// <param name="b">The byte.</param>
        /// <param name="index">The bit index to read</param>
        /// <param name="length">The bit length to read</param>
        /// <returns>bits value</returns>
        public byte GetBits(byte b, int index, int length)
        {
            int bit = 0;
            int tempBit = 0;

            if ((index >= 8) || (length > 8))
            {
                throw new Exception("The range for index or length should be 0~7.");
            }

            for (int i = 0; i < length; i++)
            {
                tempBit = ((b & (1 << (7 - index - i))) > 0) ? 1 : 0;
                bit = (bit << 1) | tempBit;
            }

            return (byte)bit;
        }

        /// <summary>
        /// Convert an array T to array T?
        /// </summary>
        /// <typeparam name="T">The type used to convert</typeparam>
        /// <param name="array">the special type of array value used to convert</param>
        /// <returns>A special type null-able list</returns>
        public T?[] ConvertArray<T>(T[] array) where T : struct
        {
            T?[] nullableArray = new T?[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                nullableArray[i] = array[i];
            }

            return nullableArray;
        }

        /// <summary>
        /// Convert a value to PropertyDataType
        /// </summary>
        /// <param name="typeValue">The type value</param>
        /// <returns>PropertyDataType type</returns>
        public PropertyDataType ConvertToPropType(ushort typeValue)
        {
            return (PropertyDataType)(typeValue & (ushort)~PropertyDataTypeFlag.MultivalueInstance);
        }

        /// <summary>
        /// Read an Int16 value from stream
        /// </summary>
        /// <returns>An Int16 value</returns>
        protected short ReadINT16()
        {
            int value;
            int b1, b2;
            b1 = this.ReadByte();
            b2 = this.ReadByte();
            value = (b2 << 8) | b1;

            return (short)value;
        }

        /// <summary>
        /// Read an Int32 value from stream
        /// </summary>
        /// <returns>An Int32 value</returns>
        protected int ReadINT32()
        {
            long value;
            int b1, b2, b3, b4;
            b1 = this.ReadByte();
            b2 = this.ReadByte();
            b3 = this.ReadByte();
            b4 = this.ReadByte();

            value = (b4 << 24) | (b3 << 16) | (b2 << 8) | b1;

            return (int)value;
        }

        /// <summary>
        /// Read an long value from stream
        /// </summary>
        /// <returns>An long value</returns>
        protected long ReadINT64()
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
        protected bool ReadBoolean()
        {
            return this.ReadByte() != 0x00;
        }

        /// <summary>
        /// Read a byte value from stream
        /// </summary>
        /// <returns>A byte</returns>
        protected byte ReadByte()
        {
            int value = this.stream.ReadByte();

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
            Guid guid = new Guid(this.ReadBytes(16));

            if (guid == null)
            {
                throw new Exception();
            }

            return guid;
        }

        /// <summary>
        /// Read an UShort value from stream
        /// </summary>
        /// <returns>An UShort value</returns>
        protected ushort ReadUshort()
        {
            int value;
            int b1, b2;
            b1 = this.ReadByte();
            b2 = this.ReadByte();

            value = (b2 << 8) | b1;

            return (ushort)value;
        }

        /// <summary>
        /// Read an UInt value from stream
        /// </summary>
        /// <returns>An UInt value</returns>
        protected uint ReadUint()
        {
            long value;
            int b1, b2, b3, b4;
            b1 = this.ReadByte();
            b2 = this.ReadByte();
            b3 = this.ReadByte();
            b4 = this.ReadByte();

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
        /// Read string value from stream according to string terminator and Encoding method
        /// </summary>
        /// <param name="encoding">The character Encoding</param>
        /// <param name="terminator">The string terminator</param>
        /// <param name="stringlength">The string length.</param>
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
                if (encoding == Encoding.Unicode && reducedUnicode)
                {
                    while (!terminated)
                    {
                        byte[] tempbytes = new byte[2];
                        tempbytes[0] = this.ReadByte();

                        if (Encoding.ASCII.GetChars(tempbytes, 0, 1)[0].ToString() == "\0")
                        {
                            terminated = true;
                            break;
                        }

                        tempbytes[1] = this.ReadByte();
                        char[] chars = Encoding.Unicode.GetChars(tempbytes, 0, 2);
                        value.Append(chars);
                    }

                    result = value.ToString();
                }
                else
                {
                    while (!terminated)
                    {
                        value.Append(this.ReadChar(encoding));

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
                    value.Append(this.ReadChar(encoding));
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

            int count = this.stream.Read(bytes, 0, length);

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
            int count = this.stream.Read(bytes, 0, length);

            if (count == -1)
            {
                throw new Exception();
            }

            char[] chars = encoding.GetChars(bytes, 0, count);

            length = encoding.GetByteCount(chars, 0, 1);

            if (length < count)
            {
                this.stream.Seek(length - count, SeekOrigin.Current);
            }

            return chars[0];
        }

        /// <summary>
        /// Converts a simple (non-flag) enum to string. If the value is not present in the underlying enum, converts to a hex string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string EnumToString(object obj)
        {
            if (Enum.IsDefined(obj.GetType(), obj))
            {
                return obj.ToString();
            }
            else
            {
                return $"0x{Convert.ToUInt64(obj):X}";
            }
        }

        /// <summary>
        /// Record start position and byte counts consumed 
        /// </summary>
        public class Position
        {
            /// <summary>
            /// Int value specifies field start position
            /// </summary>
            public int StartIndex;

            /// <summary>
            /// Int value specifies field length
            /// </summary>
            public int Offset;

            /// <summary>
            /// Boolean value specifies if field is in the compressed payload
            /// </summary>
            public bool IsCompressedXOR;

            /// <summary>
            /// Boolean value specifies if field is in the auxiliary payload
            /// </summary>
            public bool IsAuxiliayPayload;

            /// <summary>
            /// Int value specifies the buffer index of a field
            /// </summary>
            public int BufferIndex = 0;

            /// <summary>
            /// Initializes a new instance of the Position class
            /// </summary>
            /// <param name="startIndex">The start position of field</param>
            /// <param name="offset">The Length of field </param>
            public Position(int startIndex, int offset)
            {
                this.StartIndex = startIndex;
                this.Offset = offset;
                this.IsAuxiliayPayload = false;
            }
        }
    }

    /// <summary>
    /// Custom attribute for bit length
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BitAttribute : System.Attribute
    {
        /// <summary>
        /// Specify the length in bit 
        /// </summary>
        public readonly int BitLength;

        /// <summary>
        /// Initializes a new instance of the BitAttribute class
        /// </summary>
        /// <param name="bitLength">Specify the length in bit </param>
        public BitAttribute(int bitLength)
        {
            this.BitLength = bitLength;
        }
    }

    /// <summary>
    /// Custom attribute for bytes length
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BytesAttribute : System.Attribute
    {
        /// <summary>
        /// Specify the length in byte 
        /// </summary>
        public readonly uint ByteLength;

        /// <summary>
        /// Initializes a new instance of the BytesAttribute class
        /// </summary>
        /// <param name="byteLength">Specify the length in byte </param>
        public BytesAttribute(uint byteLength)
        {
            this.ByteLength = byteLength;
        }
    }
}