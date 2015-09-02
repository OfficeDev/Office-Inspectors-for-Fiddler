using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MAPIInspector.Parsers
{
    class RawData
    {
        private ulong currentBitPosition;
        private int currentBytePosition;
        private ulong remainingBitLength;
        private int remainingByteLength;
        private byte[] data;

        public RawData(byte[] rawData)
        {
            this.CurrentBitPosition = 0;
            this.RemainingBitLength = (ulong)rawData.Length << 3;
            this.Data = rawData;
        }

        public RawData(byte[] rawData, ulong currentPosition, ulong leftLength)
        {
            this.CurrentBitPosition = currentPosition;
            this.RemainingBitLength = leftLength << 3;
            this.Data = rawData;
        }

        public ulong CurrentBitPosition
        {
            get;
            set;
        }

        public ulong RemainingBitLength
        {
            get;
            set;
        }

        public byte[] Data
        {
            get;
            set;
        }

        #region Consumption Methods

        /// <summary>
        /// Read the next single bit from the input buffer.
        /// 
        /// Will throw an exception if we attempt to read past the end of the buffer.
        /// </summary>
        /// <returns>1 or 0</returns>
        public int ConsumeBit(int bitPosition = 1)
        {
            // Verify that we have not read past the end of the frame.
            if (this.RemainingBitLength < 1)
            {
                throw new Exception("There is no insufficient data to parse.");
            }

            int result = (Data[this.CurrentBitPosition>>3] & (1 << bitPosition - 1)) != 0 ? 1 : 0;
            this.CurrentBitPosition++;
            this.RemainingBitLength--;
            return result;
        }

        /// <summary>
        /// Read a single byte. This method will return a byte (or a partial byte padded with zeros) based on the total bits
        /// that we require for a given type.
        /// 
        /// Will throw an exception if we attempt to read past the end of the buffer.
        /// </summary>
        /// <param name="totalBits">the total bits we expect to read</param>
        /// <returns>a complete byte if totalBits == 0 || >= 8.  Partial byte returned with padded zeros otherwise.</returns>
        public byte ConsumeByte(ulong totalBits = 0UL)
        {
            return ConsumeByteWithWidth(totalBits);
        }

        /// <summary>
        /// Consumes the byte of the specified bit width.
        /// </summary>
        private byte ConsumeByteWithWidth(ulong widthInBits)
        {
            // Verify that we have not read past the end of the frame.
            if ((ulong)this.RemainingBitLength < widthInBits)
            {
                throw new Exception("There is no insufficient data to parse.");
            }

            // First we check if we need to return a fictionally padded value to allow for width.
            // Do we have 8 more bits to read? May be less due to our width.
            if (widthInBits > 0)
            {
                // TODO: Negative values here will find themselves padded with zeros.
                // We should be smart enough to pad '1's in this case.

                //ulong bitsRead = bitOffset - currentBitStart;
                ulong bitsNeeded = widthInBits;
                if (bitsNeeded < 8)
                {
                    // We need to read the individual bits now and fiction up
                    // a byte we can use.
                    byte partialData = 0;
                    byte[] masks = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02 };
                    for (ulong bit = 0; bit < bitsNeeded; bit++)
                    {
                        if (ConsumeBit() == 1)
                        {
                            partialData |= masks[bit];
                        }
                    }
                    return partialData;
                }
            }
            byte returnValue = 0;
            // Can I read directly?
            if (((this.CurrentBitPosition) & 7) == 0)
            {
                returnValue = this.Data[(int)this.CurrentBitPosition >> 3]; // TODO: Need an enumerator that can support LONGs
                this.CurrentBitPosition += 8;
                this.RemainingBitLength -= 8;
            }
            else
            {
                byte[] masks = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
                for (int x = 0; x < 8; x++)
                {
                    if (ConsumeBit() == 1)
                    {
                        returnValue |= masks[x];
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Read x bytes from the buffer.
        /// </summary>
        /// <param name="count">The number of BYTES to consume</param>
        /// <returns>An array of the appropriate number of bytes.</returns>
        public byte[] ConsumeBytes(int count)
        {
            // If they want a list of nothing, we provide it.
            if (count == 0)
            {
                return new byte[] { };
            }

            // Memory allocations in .NET are very fast, so let's allocate memory upfront and let GC reclaim it in case of errors.
            // TODO: The only potential problem is allocations > 85K because they go to LOH. A special logic needed to handle such cases.
            // E.g. allocating big byte arrays in chunks

            var result = new byte[count];
            var byteWidth = 8;
            for (int i = 0; i < count; i++)
            {
                var b = ConsumeByteWithWidth((ulong)byteWidth);
                result[i] = b;
            }

            return result;
        }

        /// <summary>
        /// Read an Unsigned16 from the input buffer.
        /// </summary>
        /// <returns>An unsigned short consumed from the appropriate number of bits.  Unused bits are filled with zeros.</returns>
        public ushort ConsumeUnsigned16()
        {
            byte firstByte = ConsumeByte();
            byte secondByte = ConsumeByte();
            uint value = secondByte;
            value = value << 8;
            value = value | firstByte;
            return (ushort)value;
        }

        /// <summary>
        /// Read an Unsigned32 from the input buffer.
        /// </summary>
        /// <returns>An Unsigned32 integer is consumed from the appropriate number of bits.   Unused bits are filled with zeros.</returns>
        public uint ConsumeUnsigned32()
        {
            ushort firstU16 = ConsumeUnsigned16();
            ushort secondU16 = ConsumeUnsigned16();
            uint value = secondU16;
            value = value << 16;
            value = value | firstU16;
            return value;
        }

        /// <summary>
        /// Read an Unsigned64 from the input buffer.
        /// </summary>
        /// <returns>An Unsigned64 long is consumed from the appropriate number of bits.   Unused bits are filled with zeros.</returns>
        public ulong ConsumeUnsigned64()
        {
            uint firstU32 = ConsumeUnsigned32();
            uint secondU32 = ConsumeUnsigned32();
            ulong value = secondU32;
            value = value << 32;
            value = value | firstU32;
            return value;
        }

        /// <summary>
        /// Consume a character from the input stream based on the appropriate text encoding.
        /// </summary>
        /// <returns>The consumed character.</returns>
        public char ConsumeCharacter()
        {
            return ConsumeCharacter(Encoding.GetEncoding("ASCII").GetDecoder());
        }

        /// <summary>
        /// Consumes the character using the given decoder and appends it to the provided <see cref="StringBuilder"/>.
        /// </summary>
        /// <returns>
        /// <see lang="true"/> if just consumed character is a zero-terminator; otherwise <see lang="false"/>.
        /// </returns>
        private bool ConsumeCharacterWithBoolResult(StringBuilder sb, Decoder decoder)
        {
            var c = ConsumeCharacter(decoder);
            // BUGBUG: This should compare against the default value for a TerminationCharacter as per the aspect.
            //         How do I retrieve default values from aspects when no aspect is provided?
            if (c == '\0')
            {
                return true;
            }

            sb.Append(c);
            return false;
        }

        /// <summary>
        /// Consumes the character using the given decoder.
        /// </summary>
        private char ConsumeCharacter(Decoder decoder)
        {
            var chars = new char[1]; // one character.
            int charCount = 0;
            while (charCount < 1)
            {
                charCount = decoder.GetChars(ConsumeBytes(1), 0, 1, chars, 0);
            }

            return chars[0];
        }

        /// <summary>
        /// Try to decode the message to the specific data types.
        /// </summary>
        /// <typeparam name="T">The type to decode</typeparam>
        /// <returns>The object with parsed result</returns>
        public object Parser<T> ()
        {
            Type type = typeof(T);
            bool isBasicType = Enum.IsDefined(typeof(DataType), type.Name.ToString());
            object created = null;
            ulong offset = 0;
            if (isBasicType)
            {
                object basicType = Activator.CreateInstance(type);
                DataType dataType = (DataType)Enum.Parse(typeof(DataType), type.Name.ToString());
                var result = ConsumeUsingKind(dataType, out offset);
                return (T)Convert.ChangeType(result, typeof(T));
            }
            else
            {
                FieldInfo[] fields = type.GetFields();
                PropertyInfo[] props = type.GetProperties();
                created = Activator.CreateInstance(type);
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].FieldType.Name == DataType.Boolean.ToString())
                    {
                        bool result = false;
                        result = (bool)ConsumeUsingKind(RawData.DataType.Boolean, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.Byte.ToString())
                    {
                        byte result;
                        result = (byte)ConsumeUsingKind(RawData.DataType.Byte, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.Char.ToString())
                    {
                        char result;
                        result = (char)ConsumeUsingKind(RawData.DataType.Char, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.Single.ToString())
                    {
                        float result;
                        result = (float)ConsumeUsingKind(RawData.DataType.Single, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.Double.ToString())
                    {
                        double result;
                        result = (double)ConsumeUsingKind(RawData.DataType.Double, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.Guid.ToString())
                    {
                        Guid result;
                        result = (Guid)ConsumeUsingKind(RawData.DataType.Guid, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.Int16.ToString())
                    {
                        Int16 result;
                        result = (Int16)ConsumeUsingKind(RawData.DataType.Int16, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.Int32.ToString())
                    {
                        Int32 result;
                        result = (Int32)ConsumeUsingKind(RawData.DataType.Int32, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.Int64.ToString())
                    {
                        Int64 result;
                        result = (Int64)ConsumeUsingKind(RawData.DataType.Int64, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else if (fields[i].FieldType.Name == DataType.String.ToString())
                    {
                        string result;
                        result = (string)ConsumeUsingKind(RawData.DataType.String, out offset);
                        fields[i].SetValue(created, result);
                    }
                    else
                    {
                        throw new Exception(string.Format("Unhandled primitive type in Unions: {0}", fields[i].FieldType.Name));
                    }
                }
            }
            return created;
        }

        /// <summary>
        /// Consume basic data types
        /// </summary>
        /// <param name="kind">The data type to consume</param>
        /// <param name="size">The offset the data type consumed</param>
        /// <returns></returns>
        public object ConsumeUsingKind(DataType kind, out ulong size)
        {
            object result;
            var before = this.CurrentBitPosition;
            switch (kind)
            {
                case DataType.Boolean:
                    result = ConsumeBool();
                    break;
                case DataType.Binary:
                    // result = ConsumeBytes((int)(BitsRemaining() >> 3)); //TODO:
                    result = ConsumeBit();
                    break;
                case DataType.Byte:
                    result = ConsumeByte();
                    break;
                case DataType.Char:
                    result = ConsumeCharacter();
                    break;
                case DataType.Single:
                    result = ConsumeSingle();
                    break;
                case DataType.Double:
                    result = ConsumeDouble();
                    break;
                case DataType.Guid:
                    result = ConsumeGuid();
                    break;
                case DataType.Int16:
                    result = (short)ConsumeUnsigned16();
                    break;
                case DataType.Int32:
                    result = (int)ConsumeUnsigned32();
                    break;
                case DataType.Int64:
                    result = (long)ConsumeUnsigned64();
                    break;
                case DataType.SByte:
                    result = (sbyte)ConsumeByte();
                    break;
                case DataType.String:
                    result = ConsumeString(Encoding.ASCII);
                    break;
                case DataType.UInt16:
                    result = ConsumeUnsigned16();
                    break;
                case DataType.UInt32:
                    result = ConsumeUnsigned32();
                    break;
                case DataType.UInt64:
                    result = ConsumeUnsigned64();
                    break;
                default:
                    throw new Exception(string.Format("Unhandled primitive type in Unions: {0}", kind));
            }

            var after = this.CurrentBitPosition;
            size = after - before;
            return result;
        }

        /// <summary>
        /// Retrieve a Boolean value. (Actually a byte. Returns true if != 0 )
        /// </summary>
        /// <returns>True if the byte is not 0x00 </returns>
        public bool ConsumeBool()
        {
            return ConsumeByte() != 0x00;
        }

        /// <summary>
        /// Retrieve a set number of bytes. If the total number of bytes are not present in the stream, pad
        /// pad the result.
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        public byte[] ConsumeForSure(int byteCount)
        {
            byte[] result = ConsumeBytes(byteCount);
            if (result.Length < byteCount)
            {
                Array.Resize<byte>(ref result, byteCount);
                // BUGBUG: This check is not used for determining if we have run out of data in all 
                // cases however, we need to be aware that we are generating data.
                // This should be handled more comprehensively post-CTP.
            }
            return result;
        }


        /// <summary>
        /// Read a single precision floating point number from the next four bytes.
        /// 
        /// This method using System.BitConverter to perform the conversion.
        /// </summary>
        /// <returns>the single precision floating point value</returns>
        public float ConsumeSingle()
        {
            float singleValue = BitConverter.ToSingle(ConsumeForSure(4), 0);
            return singleValue;
        }

        /// <summary>
        /// Read a single precision floating point number from up the next eight bytes.
        /// 
        /// This method using System.BitConverter to perform the conversion.
        /// </summary>
        /// <returns>the single precision floating point value</returns>
        public double ConsumeDouble()
        {
            double doubleValue = BitConverter.ToDouble(ConsumeForSure(8), 0);
            return doubleValue;
        }

        public Guid ConsumeGuid()
        {
            var g = new Guid(ConsumeForSure(16));
            return g;
        }

        /// <summary>
        /// Read a string
        /// 
        /// Requires either a length or a terminator given using aspects.
        /// </summary>
        /// <returns> the string</returns>
        public string ConsumeString(Encoding ed, string TextTerminator = "/0", uint length = 0)
        {
            ulong bitsNeeded = 16;

            if (ed != null)
            {
                if (ed == Encoding.ASCII)
                {
                    bitsNeeded = 8;
                }
                else if (ed == Encoding.Unicode)
                {
                    bitsNeeded = 16;
                }
            }

            if (bitsNeeded > this.RemainingBitLength)
            {
                return "";
            }

            if (length != 0)
            {                
                return ConsumeStringWithLength(bitsNeeded, length, ed);
            }

            var terminatorAspectValue = String.Empty;
            if (TextTerminator != string.Empty)
            {
                terminatorAspectValue = TextTerminator;
            }

            var sb = new StringBuilder();
            var done = false;

            do
            {
                if (ed == Encoding.ASCII)
                {
                    done = ConsumeCharacterWithBoolResult(sb, ed.GetDecoder());
                }
                else
                {
                    if (TextTerminator != null)
                    {
                        char c = ConsumeCharacter(ed.GetDecoder());
                        sb.Append(c);
                        // BUGBUG: Making the string and using EndsWith can be more performant.
                        // TODO : It would be much more efficient if we support only single char terminators
                        if (sb.ToString().EndsWith(terminatorAspectValue))
                        {
                            // Remove the terminator from the 'string' data.
                            sb = sb.Remove(sb.Length - terminatorAspectValue.Length, terminatorAspectValue.Length);
                            done = true;
                        }
                    }
                    else
                    {
                        done = ConsumeCharacterWithBoolResult(sb, ed.GetDecoder());
                    }
                }
            } while (!done && this.RemainingBitLength >= bitsNeeded);
            return sb.ToString();
        }

        /// <summary>
        /// Consumes the string of the specified length and character width.
        /// </summary>
        private string ConsumeStringWithLength(ulong bitsPerChar, uint lengthInChars, Encoding encoding)
        {
            // TODO : byte length calculation is done assuming that BinaryEncoding.Width is not set.
            // Is it a valid assumption? Most likely it is. Otherwise what byte width should be used?
            // The one specified by encoding or the one specified by BinaryEncoding.Width?

            var lengthInBytes = bitsPerChar * lengthInChars >> 3;
            var bytes = ConsumeBytes((int)lengthInBytes);
            var result = encoding.GetString(bytes).TrimEnd("\0".ToCharArray());
            return result;
        }

        #endregion


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

    }
}
