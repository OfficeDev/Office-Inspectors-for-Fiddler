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
    /// BaseStructure2 class (clone of BaseStructure for experimentation)
    /// </summary>
    public abstract class BaseStructure2
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

        public static TreeNode AddNodesForTree(string nodeName, object obj, int startIndex, out int offset)
        {
            Type t = obj.GetType();
            int current = startIndex;
            TreeNode res = new TreeNode(t.Name);

            if (obj is AnnotatedData ad)
            {
                offset = ad.Size;
                res.Text = ad.ToString();
                foreach (var parsedValue in ad.parsedValues)
                {
                    var alternateParsingNode = new TreeNode($"{parsedValue.Key}:{parsedValue.Value}");
                    alternateParsingNode.Tag = new Position(current, offset);
                    res.Nodes.Add(alternateParsingNode);
                }

                return res;
            }

            if (Enum.IsDefined(typeof(DataType), t.Name))
            {
                TreeNode node = new TreeNode();
                node.Text = obj.ToString();
                res.Nodes.Add(node);
            }
            else
            {
                FieldInfo[] info = t.GetFields();
                int bitLength = 0;

                if (obj is PropValue || obj is NamedPropInfo)
                {
                    info = MoveFirstNFieldsBehind(info, info.Length - 2);
                }

                for (int i = 0; i < info.Length; i++)
                {
                    int os = 0;
                    Type type = info[i].FieldType;

                    if (type.Name == "Object" && info[i].GetValue(obj) != null)
                    {
                        type = info[i].GetValue(obj).GetType();
                    }

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GetGenericArguments()[0];
                    }

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
                            else if (type.Name == "String")
                            {
                                os = 0;
                                if (obj is RgbOutputBuffer buffer)
                                {
                                    os = buffer.RPCHEADEREXT.Size;
                                }
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
                        Type fieldType = type;

                        TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, Utilities.EnumToString(info[i].GetValue(obj))));
                        res.Nodes.Add(tn);

                        if (type.Name == "String")
                        {
                            os = ((string)info[i].GetValue(obj)).Length;
                        }
                        else if (info[i].GetCustomAttributesData().Count != 0 && info[i].GetCustomAttributes(typeof(BitAttribute), false) != null)
                        {
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
                        Type elementType = type.GetElementType();
                        if (!type.IsValueType && type.GetGenericArguments().Length > 0)
                        {
                            elementType = type.GetGenericArguments()[0];
                        }

                        if (Enum.IsDefined(typeof(DataType), elementType.Name))
                        {
                            Array arr = (Array)info[i].GetValue(obj);

                            if (arr != null && arr.Length != 0)
                            {
                                StringBuilder result = new StringBuilder();

                                if (arr.Length == 6 && arr.GetType().ToString() == "System.Byte[]" && info[i].Name == "GlobalCounter")
                                {
                                    byte[] tempbytes = (byte[])info[i].GetValue(obj);
                                    result.Append("0x");

                                    foreach (byte tempbye in tempbytes)
                                    {
                                        result.Append(tempbye.ToString("X2"));
                                    }
                                }
                                else if (arr.Length > 0)
                                {
                                    result.Append(Utilities.ConvertArrayToHexString(arr));
                                }

                                TreeNode tn = new TreeNode($"{info[i].Name}:{result.ToString()}");
                                res.Nodes.Add(tn);

                                if (!(obj is PtypBinary))
                                {
                                    tn.Nodes.Add(new TreeNode($"cb:{arr.Length}"));
                                }

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
                                            if (a.GetType().Name == "RgbOutputBuffer[]" || a.GetType().Name == "ExtendedBuffer_Input[]")
                                            {
                                                compressBufferindex += 1;
                                            }

                                            tn = AddNodesForTree(fieldNameForAut, a[k], current, out os);
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
                        if (info[i].GetValue(obj) != null)
                        {
                            string fieldName = info[i].Name;
                            TreeNode node = new TreeNode();

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

                            if (fieldName == "Payload" && IsCompressedXOR)
                            {
                                RPC_HEADER_EXT header = (RPC_HEADER_EXT)info[0].GetValue(obj);
                                node = AddNodesForTree(fieldName, info[i].GetValue(obj), current, out os);
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
                                    compressBufferindex -= 1;
                                }

                                node = AddNodesForTree(fieldName, info[i].GetValue(obj), current, out os);
                                Position nodePosition = new Position(current, os);
                                node.Tag = nodePosition;
                            }

                            if (fieldName == "TransferBuffer" || fieldName == "TransferData")
                            {
                                fieldName = string.Format(fieldName + ": " + info[i].GetValue(obj).GetType().Name);
                            }

                            var toString = info[i].GetValue(obj).ToString();
                            if (!string.IsNullOrEmpty(toString))
                            {
                                node.Text = $"{fieldName}: {toString}";
                            }
                            else
                            {
                                node.Text = fieldName;
                            }

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

        public virtual void Parse(Stream s)
        {
            this.stream = s;
        }

        public override string ToString()
        {
            return string.Empty;
        }

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

        public T?[] ConvertArray<T>(T[] array) where T : struct
        {
            T?[] nullableArray = new T?[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                nullableArray[i] = array[i];
            }

            return nullableArray;
        }

        public PropertyDataType ConvertToPropType(ushort typeValue)
        {
            return (PropertyDataType)(typeValue & (ushort)~PropertyDataTypeFlag.MultivalueInstance);
        }

        protected short ReadINT16()
        {
            int value;
            int b1, b2;
            b1 = this.ReadByte();
            b2 = this.ReadByte();
            value = (b2 << 8) | b1;

            return (short)value;
        }

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

        protected long ReadINT64()
        {
            long low = this.ReadINT32();
            long high = this.ReadINT32();

            return (long)((high << 32) | low);
        }

        protected bool ReadBoolean()
        {
            return this.ReadByte() != 0x00;
        }

        protected byte ReadByte()
        {
            int value = this.stream.ReadByte();

            if (value == -1)
            {
                throw new Exception();
            }

            return (byte)value;
        }

        protected Guid ReadGuid()
        {
            Guid guid = new Guid(this.ReadBytes(16));

            if (guid == null)
            {
                throw new Exception();
            }

            return guid;
        }

        protected ushort ReadUshort()
        {
            int value;
            int b1, b2;
            b1 = this.ReadByte();
            b2 = this.ReadByte();

            value = (b2 << 8) | b1;

            return (ushort)value;
        }

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

        protected ulong ReadUlong()
        {
            long low = (uint)this.ReadUint();
            long high = (uint)this.ReadUint();

            return (ulong)(high << 32 | low);
        }

        protected string ReadString(Encoding encoding, string terminator = "\0", int stringlength = 0, bool reducedUnicode = false)
        {
            string result = null;
            StringBuilder value = new StringBuilder();

            if (stringlength == 0)
            {
                int length = terminator.Length;
                bool terminated = false;

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

        protected long RemainingBytes()
        {
            return this.stream.Length - this.stream.Position;
        }

        public class Position
        {
            public int StartIndex;
            public int Offset;
            public bool IsCompressedXOR;
            public bool IsAuxiliayPayload;
            public int BufferIndex = 0;

            public Position(int startIndex, int offset)
            {
                this.StartIndex = startIndex;
                this.Offset = offset;
                this.IsAuxiliayPayload = false;
            }
        }
    }

}