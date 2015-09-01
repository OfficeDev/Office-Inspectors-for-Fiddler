using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


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

        public virtual void AddTreeChildren(TreeNode node)
        {            
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
    }
}