using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq.Expressions;


namespace MAPIInspector.Parsers
{
    public abstract class BaseStructure
    {
        /// <summary>
        /// The stream to parse
        /// </summary>
        private Stream stream;

        /// <summary>
        /// Boolean value, if payload is compressed or obfascated, value is true. otherwise, value is false.
        /// </summary>
        public static bool isCompressedXOR = false;

        /// <summary>
        /// Parse stream to specific message
        /// </summary>
        /// <param name="s">Stream to parse</param>
        public virtual void Parse(Stream s)
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

            for (int i = 0; i < length; i++)
            {
                tempBit = ((b & (1 << (7 - index - i))) > 0) ? 1 : 0;
                Bit = (Bit << 1) | tempBit;
            }
            return (byte)Bit;
        }

        /// <summary>
        /// Read an Int16 value from stream
        /// </summary>
        /// <returns>An Int16 value</returns>
        protected Int16 ReadINT16()
        {
            int value;
            int b1, b2;
            b1 = stream.ReadByte();
            b2 = stream.ReadByte();

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
            b1 = stream.ReadByte();
            b2 = stream.ReadByte();
            b3 = stream.ReadByte();
            b4 = stream.ReadByte();

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
            b1 = stream.ReadByte();
            b2 = stream.ReadByte();

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
            b1 = stream.ReadByte();
            b2 = stream.ReadByte();
            b3 = stream.ReadByte();
            b4 = stream.ReadByte();

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
                string terminator = (string)infoString[3].GetValue(obj);
                TreeNode node = new TreeNode(string.Format("{0}:{1}", infoString[0].Name, infoString[0].GetValue(obj)));
                // If the StringLength is not equal 0, the StringLength will be os value.
                if (infoString[4].GetValue(obj).ToString() != "0")
                {
                    os = ((int)infoString[4].GetValue(obj));
                }
                // If the Encoding is Unicode.
                else if (infoString[2].GetValue(obj).ToString() == "System.Text.UnicodeEncoding")
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
                //If the Encoding is ASCII.
                else
                {
                    if (infoString[0].GetValue(obj) != null)
                    {
                        os = ((string)infoString[0].GetValue(obj)).Length;
                    }
                    os += terminator.Length;
                }

                offset = os;
                Position positionString = new Position(current, os);
                node.Tag = positionString;
                res.Nodes.Add(node);
                res.Tag = positionString;
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
                int BitLength = 0;

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

                    // If the field type is nullable simple data type (such as int?), set the field data type as basic data type (such as int in int?)
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GetGenericArguments()[0];
                    }

                    // Check whether the field data type is simple type: 
                    // Boolean, Byte, Char, Double, Decimal,Single, Guid, Int16, Int32, Int64, SByte, String, UInt16, UInt32, UInt64, DateTime
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
                            // Check if it is bit.
                            else if (type.Name == "Byte" && info[i].GetCustomAttributesData().Count != 0 && info[i].GetCustomAttributes(typeof(BitAttribute), false) != null)
                            {
                                BitAttribute attribute = (BitAttribute)info[i].GetCustomAttributes(typeof(BitAttribute), false)[0];
                                if (BitLength % 8 == 0)
                                {
                                    os += 1;
                                }
                                else
                                {
                                    current -= 1;
                                    os += 1;
                                }
                                BitLength += attribute.BitLength;
                            }
                            else if (type.Name != "Boolean")
                            {
                                os = Marshal.SizeOf(fieldType);
                            }
                            else
                            {
                                os = sizeof(Boolean);
                            }
                            Position ps = new Position(current, os);
                            tn.Tag = ps;
                            current += os;
                        }
                    }
                    // Else if the field data type is enum data type, its underlying type is simple type and its value is not null, calculate each field's offset
                    // and length. There are two situations: one is string type, we should calculate it's actual length (via getting value); another one is calculating
                    // the size of underlying type of enum. 
                    else if ((type.IsEnum && Enum.IsDefined(typeof(DataType), type.GetEnumUnderlyingType().Name)) && info[i].GetValue(obj) != null)
                    {
                        Type fieldType = type;
                        // TODO: display decimal value to hex one
                        TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, info[i].GetValue(obj).ToString()));
                        res.Nodes.Add(tn);
                        if (type.Name == "String")
                        {
                            os = ((string)info[i].GetValue(obj)).Length;
                        }
                        // Modify the bit os for the NotificationFlagsT in MSOXCNOTIF
                        else if (info[i].GetCustomAttributesData().Count != 0 && info[i].GetCustomAttributes(typeof(BitAttribute), false) != null)
                        {
                            BitAttribute attribute = (BitAttribute)info[i].GetCustomAttributes(typeof(BitAttribute), false)[0];
                            if ((BitLength) % 8 != 0)
                            {
                                current -= 1;
                            }
                            if (attribute.BitLength % 8 == 0)
                            {
                                os += attribute.BitLength / 8;
                            }
                            else
                            {
                                os += attribute.BitLength / 8 + 1;
                            }
                            BitLength += attribute.BitLength;
                        }
                        else
                        {
                            os = Marshal.SizeOf(fieldType.GetEnumUnderlyingType());
                        }

                        Position ps = new Position(current, os);
                        tn.Tag = ps;
                        current += os;
                    }
                    // If the field type is array, there are two properties need to know: optional or required, array element data type is simple or complex
                    // Field value considered: empty, null or value type displaying when not null/empty                  
                    else if (type.IsArray)
                    {
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
                                    byte[] tempbytes = (System.Byte[])info[i].GetValue(obj);
                                    result.Append("0x");
                                    foreach (byte tempbye in tempbytes)
                                    {
                                        result.Append(tempbye.ToString("X2"));
                                    }
                                }
                                else
                                {
                                    result.Append("[");
                                    foreach (var ar in arr)
                                    {
                                        result.Append(ar.ToString() + ",");
                                    }
                                    result.Remove(result.Length - 1, 1);
                                    result.Append("]");
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
                            else if (arr != null && arr.Length == 0)
                            {
                                StringBuilder result = new StringBuilder("[]");
                                TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, result.ToString()));
                                res.Nodes.Add(tn);
                            }
                        }
                        // Else if the element data type is not simple type, here will consider array type and complex type
                        else
                        {
                            Array arr = (Array)info[i].GetValue(obj);
                            object[] a = (object[])arr;
                            if (arr != null && arr.Length != 0)
                            {
                                string fieldNameForAut = info[i].Name;
                                TreeNode tnArr = new TreeNode(info[i].Name);
                                TreeNode tn = new TreeNode();
                                int arros = 0;
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
                                                tnArr.Nodes.Add(tn);
                                                os = ((byte[])a[k]).Length;
                                                ps = new Position(current, os);
                                                tn.Tag = ps;
                                            }
                                            os = ((byte[])a[k]).Length;
                                            ps = new Position(current, os);
                                            tnArr.Tag = ps;
                                        }
                                        // If the item in array is complex type, loop call the function to add it to tree.
                                        else
                                        {
                                            tn = AddNodesForTree(a[k], current, out os);
                                            tnArr.Nodes.Add(tn);
                                            Position ps = new Position(current, os);
                                            tn.Tag = ps;
                                        }
                                    }

                                    current += os;
                                    arros += os;
                                }

                                Position pss = new Position(current - arros, arros);
                                tnArr.Tag = pss;

                                // Special handling for the array field data type when its name is Payload and it's compressed or XOR: recalculating the offset and position.
                                if (fieldNameForAut == "Payload" && isCompressedXOR)
                                {
                                    tnArr = TreeNodeForCompressed(tnArr, current - arros, true);
                                    string text = tnArr.Text.Replace("Payload", "Payload(CompressedOrObfuscated)");
                                    tnArr.Text = text;

                                    // Modified the Payload size that it is compressed.
                                    RPC_HEADER_EXT header = (RPC_HEADER_EXT)info[0].GetValue(obj);
                                    Position postion = (Position)tnArr.Tag;
                                    postion.Offset = header.Size;
                                    os = postion.Offset;
                                    tnArr.Tag = postion;
                                }
                                res.Nodes.Add(tnArr);
                            }
                            // Else if the array is not null and its length is zero, only displaying [].
                            else if (arr != null && arr.Length == 0)
                            {
                                StringBuilder result = new StringBuilder("[]");
                                TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, result.ToString()));
                                res.Nodes.Add(tn);
                            }
                        }
                    }
                    // If the field type is complex type, loop call the function until finding its data type.  
                    else
                    {
                        if (info[i].GetValue(obj) != null)
                        {
                            string fieldName = info[i].Name;
                            TreeNode node = new TreeNode();

                            // The below logical is used to check whether the payload is compressed or XOR.
                            if (fieldName == "RPC_HEADER_EXT")
                            {
                                if (((ushort)((RPC_HEADER_EXT)info[i].GetValue(obj)).Flags & 0x0002) == (ushort)RpcHeaderFlags.XorMagic
                                   || ((ushort)((RPC_HEADER_EXT)info[i].GetValue(obj)).Flags & 0x0001) == (ushort)RpcHeaderFlags.Compressed)
                                {
                                    isCompressedXOR = true;
                                }
                                else
                                {
                                    isCompressedXOR = false;
                                }
                            }

                            // If the field name is Payload and its compressed, recalculating the offset and length, else directly loop call this function
                            if (fieldName == "Payload" && isCompressedXOR)
                            {
                                RPC_HEADER_EXT header = (RPC_HEADER_EXT)info[0].GetValue(obj);
                                node = AddNodesForTree(info[i].GetValue(obj), current, out os);
                                Position postion = (Position)node.Tag;
                                postion.Offset = header.Size;
                                os = postion.Offset;
                                node.Tag = postion;
                                node = TreeNodeForCompressed(node, current);
                                fieldName = "Payload(CompressedOrObfuscated)";
                            }
                            else
                            {
                                node = AddNodesForTree(info[i].GetValue(obj), current, out os);
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

        #region Helper for AddNodesForTree function
        /// <summary>
        /// Record start position and byte counts consumed 
        /// </summary>
        public class Position
        {
            public int StartIndex;
            public int Offset;
            public bool IsCompressedXOR;
            public bool IsAuxiliayPayload;
            public Position(int startIndex, int offset)
            {
                this.StartIndex = startIndex;
                this.Offset = offset;
                this.IsAuxiliayPayload = false;
            }
        }

        /// <summary>
        /// Convert an array T to array T?
        /// </summary>
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
        /// Modify the start index for the TreeNode which source data is compressed
        /// </summary>
        public static TreeNode TreeNodeForCompressed(TreeNode node, int current, bool isAux = false)
        {
            foreach (TreeNode n in node.Nodes)
            {
                TreeNode nd = n;
                if (nd.Tag != null)
                {
                    ((Position)(nd.Tag)).IsCompressedXOR = true;
                    ((Position)(nd.Tag)).StartIndex -= current;
                }
                if (nd.Nodes.Count != 0)
                {
                    TreeNodeForCompressed(nd, current, isAux);
                }
            }
            return node;
        }

        /// <summary>
        /// Moving the number of fields in FieldInfo from begining to the end
        /// </summary>
        public static FieldInfo[] MoveFirstNFieldsBehind(FieldInfo[] field, int n)
        {
            FieldInfo[] NewField = new FieldInfo[field.Length];

            if (n < 0 || n > field.Length)
            {
                throw new InvalidOperationException(string.Format("Moving Failed because the length ({0}) need to move is exceeded the fields' length ({1}).", n, field.Length));
            }
            else
            {
                int i = 0;
                for (; i < field.Length - n; i++)
                {
                    NewField[i] = field[n + i];
                }

                for (; i < field.Length; i++)
                {
                    NewField[i] = field[i - (field.Length - n)];
                }
                return NewField;
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

        /// <summary>
        /// Convert a value to PropertyDataType
        /// </summary>
        /// <param name="typeValue"></param>
        /// <returns>PropertyDataType type</returns>
        public PropertyDataType ConvertToPropType(ushort typeValue)
        {
            return (PropertyDataType)(typeValue & (ushort)~PropertyDataTypeFlag.MultivalueInstance);
        }
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
    /// Custom attribute for bytes length
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BytesAttribute : System.Attribute
    {
        public readonly uint ByteLength;
        public BytesAttribute(uint byteLength)
        {
            this.ByteLength = byteLength;
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