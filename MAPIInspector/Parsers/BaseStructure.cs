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
        private Stream stream;

        public virtual void Parse(Stream s)
        {
            stream = s;
        }

        public override string ToString()
        {
            return "";
        }

        protected byte ReadByte()
        {
            int value = stream.ReadByte();
            if (value == -1)
            {
                throw new Exception();
            }
            return (byte)value;
        }

        protected ushort ReadUshort()
        {
            int value;
            int b1, b2;
            b1 = stream.ReadByte();
            b2 = stream.ReadByte();

            if ((b1 == -1) || (b2 == -1))
            {
                throw new Exception();
            }

            value = (b2 << 8) + b1;

            return (ushort)value;
        }

        protected uint ReadUint()
        {
            long value;
            int b1, b2, b3, b4;
            b1 = stream.ReadByte();
            b2 = stream.ReadByte();
            b3 = stream.ReadByte();
            b4 = stream.ReadByte();

            if ((b1 == -1) || (b2 == -1) || (b3 == -1) || (b4 == -1))
            {
                throw new Exception();
            }

            value = (b4 << 24) + (b3 << 16) + (b2 << 8) + b1;

            return (uint)value;
        }
                
        protected string ReadString(string terminator = "\0")
        {            
            StringBuilder value = new StringBuilder();
            int length = terminator.Length;
            bool terminated = false;

            while (!terminated) 
            {
                int b = stream.ReadByte();
                if (b == -1)
                {
                    throw new Exception();
                }
                
                value.Append((char)b);
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

            return value.Remove(value.Length - length, length).ToString();
        }

        protected string ReadString(Encoding encoding, string terminator = "\0")
        {            
            StringBuilder value = new StringBuilder();
            int length = terminator.Length;
            bool terminated = false;

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
            return value.Remove(value.Length - length, length).ToString();
        }

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

        public TreeNode AddNodesForTree(object obj, int startIndex, out int offset)
        {
            Type t = obj.GetType();
            int current = startIndex;
            TreeNode res = new TreeNode(t.Name);

            if (Enum.IsDefined(typeof(DataType), t.Name))
            {
                throw new Exception("The method doesn't support handling simple data type.");
            }
            else
            {
                FieldInfo[] info = t.GetFields();
                for (int i = 0; i < info.Length; i++)
                {
                    int os = 0;
                    Type type = info[i].FieldType;
                    if (type.Name == "Object")
                    {
                        type = info[i].GetValue(obj).GetType();
                    }
                    if (Enum.IsDefined(typeof(DataType), type.Name))
                    {
                        Type fieldType = type;
                        TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, info[i].GetValue(obj).ToString()));
                        res.Nodes.Add(tn);
                        if (type.Name == "String")
                        {
                            object[] attributes = info[i].GetCustomAttributes(typeof(HelpAttribute), false);
                            if (((HelpAttribute)(attributes[0])).Encode == StringEncoding.Unicode)
                            {
                                os = ((string)info[i].GetValue(obj)).Length * 2;
                            }
                            else
                            {
                                os = ((string)info[i].GetValue(obj)).Length;
                            }

                            os += (int)((HelpAttribute)(attributes[0])).TerminatorLength;
                        }
                        else
                        {
                            os = Marshal.SizeOf(fieldType);
                        }

                        tn.Tag = new Position(current, os);
                        current += os;
                    }
                    else if ((type.IsEnum && Enum.IsDefined(typeof(DataType), type.GetEnumUnderlyingType().Name)))
                    {
                        Type fieldType = type;
                        TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, info[i].GetValue(obj).ToString()));
                        res.Nodes.Add(tn);
                        if (type.Name == "String")
                        {
                            os = ((string)info[i].GetValue(obj)).Length;
                        }
                        else
                        {
                            os = Marshal.SizeOf(fieldType.GetEnumUnderlyingType());
                        }
                        tn.Tag = new Position(current, os);
                        current += os;
                    }
                    else if (type.IsArray)
                    {
                        if (Enum.IsDefined(typeof(DataType), type.GetElementType().Name))
                        {
                            Array arr = (Array)info[i].GetValue(obj);
                            if (arr != null)
                            {
                                StringBuilder result = new StringBuilder("[");
                                foreach (var ar in arr)
                                {
                                    result.Append(ar.ToString() + ",");
                                }
                                result.Remove(result.Length - 1, 1);
                                result.Append("]");
                                TreeNode tn = new TreeNode(string.Format("{0}:{1}", info[i].Name, result.ToString()));
                                res.Nodes.Add(tn);

                                if (type.GetElementType().Name == "String")
                                {
                                    for (int j = 0; j < arr.Length; j++)
                                    {
                                        os += ((string[])(arr))[j].Length;
                                        object[] attributes = info[i].GetCustomAttributes(typeof(HelpAttribute), false);
                                        os += (int)((HelpAttribute)(attributes[0])).TerminatorLength;
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < arr.Length; j++)
                                    {
                                        os += Marshal.SizeOf(type.GetElementType());
                                    }
                                }

                                tn.Tag = new Position(current, os);
                                current += os;
                            }
                        }
                        else
                        {
                            Array arr = (Array)info[i].GetValue(obj);
                            object[] a = (object[])arr;
                            if (arr != null)
                            {
                                TreeNode tnArr = new TreeNode(info[i].Name);
                                int arros = 0;
                                for (int k = 0; k < arr.Length; k++)
                                {
                                    
                                    TreeNode tn = AddNodesForTree(a[k], current, out os);
                                    tnArr.Nodes.Add(tn);
                                    tn.Tag = new Position(current, os);
                                    current += os;
                                    arros += os;
                                }
                                res.Nodes.Add(tnArr);
                                tnArr.Tag = new Position(current - arros, arros);
                            }
                        }
                    }
                    else
                    {
                        if (info[i].GetValue(obj) != null)
                        {
                            string filedName = info[i].Name;
                            TreeNode node = AddNodesForTree(info[i].GetValue(obj), current, out os);
                            node.Text = filedName;
                            res.Nodes.Add(node);
                            current += os;
                        }
                    }
                }
            }

            offset = current - startIndex;
            Position ps = new Position(startIndex, offset);
            res.Tag = ps;

            return res;
        }

        #region Helper for AddNodesForTree function
        public enum StringEncoding
        {
            ASCII,
            Unicode
        }

        public class Position
        {
            public int StartIndex;
            public int Offset;
            public Position(int startIndex, int offset)
            {
                this.StartIndex = startIndex;
                this.Offset = offset;
            }
        }

        [AttributeUsage(AttributeTargets.All)]
        public class HelpAttribute : System.Attribute
        {
            public readonly StringEncoding Encode;
            public readonly uint TerminatorLength;
            public HelpAttribute(StringEncoding encode, uint length = 0)
            {
                this.Encode = encode;
                this.TerminatorLength = length;
            }
        }

        /// <summary>
        /// The kind of a pattern.
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
            UInt64
        }
        #endregion
    }
}