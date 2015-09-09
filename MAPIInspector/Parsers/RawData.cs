using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace MAPIInspector.Parsers
{
    class RawData
    {
        public RawData(Stream s)
        {
            byte[] DataInBytes = new byte[s.Length];
            for (int i = 0; i < s.Length - 1; i++)
            {
                DataInBytes[i] = (byte)s.ReadByte();
            }
            this.CurrentBitPosition = 0;
            this.RemainingBitLength = (ulong)DataInBytes.Length << 3;
            this.Data = DataInBytes;
        } 

        public RawData(byte[] rawData)
        {
            this.CurrentBitPosition = 0;
            this.RemainingBitLength = (ulong)rawData.Length << 3;
            this.Data = rawData;
        }

        public RawData(byte[] rawData, ulong currentPosition)
        {
            this.CurrentBitPosition = currentPosition;
            this.Data = rawData;
        }

        public RawData(byte[] rawData, ulong currentPosition, ulong leftBitLength)
        {
            this.CurrentBitPosition = currentPosition;
            this.RemainingBitLength = leftBitLength;
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
            if (this.RemainingBitLength == 0 || this.RemainingBitLength < (ulong)bitPosition)
            {
                throw new Exception("There is no sufficient data to parse.");
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
            if (this.RemainingBitLength == 0 || this.RemainingBitLength < widthInBits)
            {
                throw new Exception("There is no sufficient data to parse.");
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
        private bool IsConsumedCharacterZeroTerminator(StringBuilder sb, Decoder decoder)
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
        /// Retrieve a Boolean value. (Actually a byte. Returns true if != 0 )
        /// </summary>
        /// <returns>True if the byte is not 0x00 </returns>
        public bool ConsumeBool()
        {
            return ConsumeByte() != 0x00;
        }

        /// <summary>
        /// Retrieve a set number of bytes. If the total number of bytes are not present in the stream, pad the result.
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
            return BitConverter.ToSingle(ConsumeForSure(4), 0);
        }

        /// <summary>
        /// Read a single precision floating point number from up the next eight bytes.
        /// 
        /// This method using System.BitConverter to perform the conversion.
        /// </summary>
        /// <returns>the single precision floating point value</returns>
        public double ConsumeDouble()
        {
           return BitConverter.ToDouble(ConsumeForSure(8), 0);       
        }

        public Guid ConsumeGuid()
        {
            return new Guid(ConsumeForSure(16));
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

            if (this.RemainingBitLength == 0 || this.RemainingBitLength < bitsNeeded)
            {
                throw new Exception("There is no sufficient data to parse.");
            }

            if (ed != null && ed == Encoding.ASCII)
            {
                bitsNeeded = 8;
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
                    done = IsConsumedCharacterZeroTerminator(sb, ed.GetDecoder());
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
                        done = IsConsumedCharacterZeroTerminator(sb, ed.GetDecoder());
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

        /// <summary>
        /// Consume basic data types
        /// </summary>
        /// <param name="kind">The data type to consume</param>
        /// <param name="size">The offset the data type consumed</param>
        /// <returns></returns>
        public object ConsumeUsingKind(DataType kind, out ulong start, out ulong size)
        {
            object result;
            var before = this.CurrentBitPosition;
            start = before;
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

        #endregion

        #region Parse method
        /// <summary>
        /// Try to decode the message to the specific data types.
        /// </summary>
        /// <typeparam name="T">The type to decode</typeparam>
        /// <returns>The object with parsed result</returns>
        public void Parse<T>(out Dictionary<object, ulong> typeResult, out Dictionary<FieldInfo, ulong> fieldsInfoStart, out Dictionary<FieldInfo, ulong> fieldsInfoLength)
        {
            fieldsInfoStart = new Dictionary<FieldInfo, ulong>();
            fieldsInfoLength = new Dictionary<FieldInfo, ulong>();
            typeResult = new Dictionary<object, ulong>();

            Type type = typeof(T);           
            bool isBasicType = Enum.IsDefined(typeof(DataType), type.Name.ToString());
            ulong startPosition = 0;
            ulong offset = 0;

            if (isBasicType)
            {
                DataType dataType = (DataType)Enum.Parse(typeof(DataType), type.Name);
                var result = ConsumeUsingKind(dataType, out startPosition, out offset);
                typeResult.Add((T)Convert.ChangeType(result, typeof(T)), offset);
            }
            else
            {
                ulong totalOffset = 0;
                FieldInfo[] fields = type.GetFields();
                ulong startLength = 0;
                object created = Activator.CreateInstance(type);
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].FieldType.IsArray)
                    {
                        object[] attributes = fields[i].GetCustomAttributes(typeof(MarshalAsAttribute), false);
                        int length = 0;
                        ulong offsetLength = 0;
                        if (attributes.Length > 0)
                        {
                            MarshalAsAttribute marshal = (MarshalAsAttribute)attributes[0];
                            length = marshal.SizeConst;
                            Type t = fields[i].FieldType.GetElementType();
                            DataType dataType = (DataType)Enum.Parse(typeof(DataType), t.Name);

                            Array arr = Array.CreateInstance(t, length);
                            for (int j = 0; j < length; j++)
                            {
                                arr.SetValue(ConsumeUsingKind(dataType, out startLength, out offset), j);
                                if (j == 0)
                                {
                                    startPosition = startLength;
                                }
                                totalOffset += offset;
                                offsetLength += offset;
                            }
                            fields[i].SetValue(created, arr);
                            fieldsInfoStart.Add(fields[i], startPosition);
                            fieldsInfoLength.Add(fields[i], offsetLength);
                        }
                    }
                    else if (Enum.IsDefined(typeof(DataType), fields[i].FieldType.Name))
                    {
                        DataType dataType = (DataType)Enum.Parse(typeof(DataType), fields[i].FieldType.Name);
                        fields[i].SetValue(created, ConsumeUsingKind(dataType, out startLength, out offset));
                        fieldsInfoStart.Add(fields[i], startLength);
                        fieldsInfoLength.Add(fields[i], offset);
                        totalOffset += offset;
                    }
                    else
                    {
                        throw new Exception(string.Format("Unhandled primitive type in Unions: {0}", fields[i].FieldType.Name));
                    }
                }
                typeResult.Add(created, totalOffset);

            }
        }

        #endregion Parse method

        public void ParseBasicDataType<T>(out object result, out ulong startPosition, out ulong offsetLength)
        {
            Type type = typeof(T);
            result = null;
            startPosition = 0;
            offsetLength = 0;

            if (Enum.IsDefined(typeof(DataType), type.Name))
            {
                DataType dataType = (DataType)Enum.Parse(typeof(DataType), type.Name);
                var parsedResult = ConsumeUsingKind(dataType, out startPosition, out offsetLength);
                result = (T)Convert.ChangeType(parsedResult, typeof(T));
            }
        }

        public void ParseArrayType(FieldInfo field, out object result, out ulong startPosition, out ulong offsetLength)
        {
            Type type = field.FieldType;
            result = null;
            startPosition = 0;
            offsetLength = 0;

            if (type.IsArray)
            {
                object[] attributesBasic = field.GetCustomAttributes(typeof(MarshalAsAttribute), false);
                int lengthBasic = 0;

                if (attributesBasic.Length > 0)
                {
                    MarshalAsAttribute marshal = (MarshalAsAttribute)attributesBasic[0];
                    lengthBasic = marshal.SizeConst;
                    Type t = type.GetElementType();
                    DataType dataType = (DataType)Enum.Parse(typeof(DataType), t.Name);

                    Array arr = Array.CreateInstance(t, lengthBasic);
                    for (int j = 0; j < lengthBasic; j++)
                    {
                        Type raw = this.GetType();
                        object[] parameters = new object[] { null, null, null };
                        string methodName = string.Empty;
                        if (Enum.IsDefined(typeof(DataType), t.Name))
                        {
                            methodName = "ParseBasicDataType";
                        }
                        else
                        {
                            methodName = "ParseAllDataType";
                        }

                        MethodInfo mi = raw.GetMethod(methodName).MakeGenericMethod(t);
                        mi.Invoke(this, parameters);

                        if (j == 0 && parameters.Length >= 3)
                        {
                            startPosition = (ulong)parameters[1];
                        }
                        arr.SetValue(parameters[0], j);
                        offsetLength += (ulong)parameters[2];
                    }
                    result = arr;
                }
            }
        }

        public void ParseAllDataType<T>(out object result, out ulong startPosition, out ulong offsetLength)
        {
            Type type = typeof(T);
            result = null;
            startPosition = 0;
            offsetLength = 0;
            ulong totalOffsetLength = 0;

            if (Enum.IsDefined(typeof(DataType), type.Name))
            {
                ParseBasicDataType<T>(out result, out startPosition, out offsetLength);
                totalOffsetLength += offsetLength;
            }
            else
            {
                FieldInfo[] fields = type.GetFields();
                object created = Activator.CreateInstance(type);
                foreach (FieldInfo f in fields)
                {
                    Type t = this.GetType();
                    object[] parameters = new object[] { null, null, null };
                    string methodName = string.Empty;
                    if (Enum.IsDefined(typeof(DataType), f.FieldType.Name))
                    {
                        methodName = "ParseBasicDataType";
                    }
                    else if (f.FieldType.IsArray)
                    {
                        parameters = new object[] { f, null, null, null };
                        methodName = "ParseArrayType";
                    }
                    else
                    {
                        methodName = "ParseAllDataType";
                    }

                    MethodInfo mi = null;
                    if (!f.FieldType.IsArray)
                    {
                         mi = t.GetMethod(methodName).MakeGenericMethod(f.FieldType);
                    }
                    else
                    {
                        mi = t.GetMethod(methodName);
                    }
                    mi.Invoke(this, parameters);
                    if (parameters.Length > 0 && !f.FieldType.IsArray)
                    {
                        startPosition = (ulong)parameters[1];
                        offsetLength = (ulong)parameters[2];
                        f.SetValue(created, parameters[0]);
                        totalOffsetLength += offsetLength;
                    }
                    else
                    {
                        startPosition = (ulong)parameters[2];
                        offsetLength = (ulong)parameters[3];
                        f.SetValue(created, parameters[1]);
                        totalOffsetLength += offsetLength;
                    }
                }
                result = created;
                offsetLength = totalOffsetLength;
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

    }
}
