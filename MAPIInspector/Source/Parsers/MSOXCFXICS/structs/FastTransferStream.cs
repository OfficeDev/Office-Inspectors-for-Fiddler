namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Used for Parsing a fast transfer stream.
    /// 2.2.4 FastTransfer Stream
    /// </summary>
    public class FastTransferStream : MemoryStream
    {
        /// <summary>
        /// The length of a GUID structure.
        /// </summary>
        public static int GuidLength = Guid.Empty.ToByteArray().Length;

        /// <summary>
        /// The length of a MetaTag property.
        /// </summary>
        private const int MetaLength = 4;

        /// <summary>
        /// Initializes a new instance of the FastTransferStream class.
        /// </summary>
        /// <param name="buffer">A bytes array.</param>
        /// <param name="writable">Whether the stream supports writing.</param>
        public FastTransferStream(byte[] buffer, bool writable)
            : base(buffer, 0, buffer.Length, writable, true)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the stream position is at the end of this stream
        /// </summary>
        public bool IsEndOfStream
        {
            get
            {
                return this.Position == this.Length;
            }
        }

        /// <summary>
        /// Read a Markers value from stream,and advance the position within the stream by 4
        /// </summary>
        /// <returns>The Markers value</returns>
        public Markers ReadMarker()
        {
            byte[] buffer = new byte[MetaLength];
            this.Read(buffer, 0, MetaLength);
            uint marker;
            marker = BitConverter.ToUInt32(buffer, 0);
            return (Markers)marker;
        }

        /// <summary>
        /// Read a byte value from stream and advance the position within the stream by 1
        /// </summary>
        /// <returns>A byte</returns>
        public new byte ReadByte()
        {
            int value = base.ReadByte();
            if (value == -1)
            {
                throw new Exception();
            }

            return (byte)value;
        }

        /// <summary>
        /// Read a UInt value from stream,and advance the position within the stream by 4
        /// </summary>
        /// <returns>The UInt value.</returns>
        public uint ReadUInt32()
        {
            byte[] buffer = new byte[4];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Read an int value from stream,and advance the position within the stream by 4
        /// </summary>
        /// <returns>The int value.</returns>
        public int ReadInt32()
        {
            byte[] buffer = new byte[4];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Read a unsigned short integer value from stream,and advance the position within the stream by 2
        /// </summary>
        /// <returns>The unsigned short integer value</returns>
        public ushort ReadUInt16()
        {
            byte[] buffer = new byte[2];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Read a short value from stream,and advance the position within the stream by 2
        /// </summary>
        /// <returns>The short value</returns>
        public short ReadInt16()
        {
            byte[] buffer = new byte[2];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Read a long value from stream,and advance the position within the stream by 8
        /// </summary>
        /// <returns>The long value</returns>
        public long ReadInt64()
        {
            byte[] buffer = new byte[8];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Read the unsigned long integer value from stream, and advance the position within the stream by 8
        /// </summary>
        /// <returns>The unsigned long integer value</returns>
        public ulong ReadUInt64()
        {
            byte[] buffer = new byte[8];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        /// Read a float value from stream, and advance the position within the stream by 4
        /// </summary>
        /// <returns>The float value</returns>
        public float ReadFloating32()
        {
            byte[] buffer = new byte[4];
            this.Read(buffer, 0, MetaLength);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Read a double value from stream,and advance the position within the stream by 8
        /// </summary>
        /// <returns>The double value</returns>
        public double ReadFloating64()
        {
            byte[] buffer = new byte[8];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Read a currency value from stream,and advance the position within the stream by 8
        /// </summary>
        /// <returns>The long value represents a currency value</returns>
        public long ReadCurrency()
        {
            return this.ReadInt64();
        }

        /// <summary>
        /// Read a FloatingTime value from stream, and advance the position within the stream by 8
        /// </summary>
        /// <returns>The double value represents a FloatingTime value</returns>
        public double ReadFloatingTime()
        {
            return this.ReadFloating64();
        }

        /// <summary>
        /// Read a Boolean value from stream, and advance the position within the stream by 2
        /// </summary>
        /// <returns>The unsigned short integer value represents a Boolean value</returns>
        public ushort ReadBoolean()
        {
            return this.ReadUInt16();
        }

        /// <summary>
        /// Read a Time value from stream,and advance the position within the stream by 2
        /// </summary>
        /// <returns>The unsigned long integer value represents a Time value</returns>
        public ulong ReadTime()
        {
            return this.ReadUInt64();
        }

        /// <summary>
        /// Read a GUID value from stream, and advance the position.
        /// </summary>
        /// <returns>The GUID value</returns>
        public Guid ReadGuid()
        {
            byte[] buffer = new byte[Guid.Empty.ToByteArray().Length];
            this.Read(buffer, 0, buffer.Length);
            return new Guid(buffer);
        }

        /// <summary>
        /// Read  bytes from stream, and advance the position.
        /// </summary>
        /// <param name="size">The size of bytes</param>
        /// <returns>The bytes array</returns>
        public byte[] ReadBlock(int size)
        {
            byte[] buffer = new byte[size];
            this.Read(buffer, 0, size);
            return buffer;
        }

        /// <summary>
        /// Read a list of blocks and advance the position.
        /// </summary>
        /// <param name="totalSize">The total number of bytes to read</param>
        /// <param name="blockSize">The size of each block</param>
        /// <returns>A list of blocks</returns>
        public byte[][] ReadBlocks(int totalSize, int blockSize)
        {
            int i;
            List<byte[]> l = new List<byte[]>();
            for (i = 0; i < totalSize; i++)
            {
                l.Add(this.ReadBlock(blockSize));
            }

            return l.ToArray();
        }

        /// <summary>
        /// Read LengthOfBlock and advance the position.
        /// </summary>
        /// <returns>A LengthOfBlock specifies the length of the bytes array</returns>
        public LengthOfBlock ReadLengthBlock()
        {
            int tmp = this.ReadInt32();
            byte[] buffer = this.ReadBlock(tmp);
            return new LengthOfBlock(tmp, buffer);
        }

        /// <summary>
        /// Read a list of LengthOfBlock and advance the position.
        /// </summary>
        /// <param name="totalLength">The number of bytes to read</param>
        /// <returns>A list of LengthOfBlock</returns>
        public LengthOfBlock[] ReadLengthBlocks(int totalLength)
        {
            int i = 0;
            List<LengthOfBlock> list = new List<LengthOfBlock>();

            while (i < totalLength)
            {
                LengthOfBlock tmp = this.ReadLengthBlock();
                i += 1;
                list.Add(tmp);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Read a list of blocks and advance the position for partial.
        /// </summary>
        /// <param name="totalSize">The total number of bytes to read</param>
        /// <param name="blockSize">The size of each block</param>
        /// <param name="type">The data type to read</param>
        /// <param name="isGetbuffer">Check whether it's RopGetBuffer</param>
        /// <param name="isPutBuffer">Check whether it's RopPutBuffer</param>
        /// <returns>A list of blocks</returns>
        public byte[][] ReadBlocksPartial(int totalSize, int blockSize, ushort type, bool isGetbuffer, bool isPutBuffer)
        {
            int i;
            List<byte[]> l = new List<byte[]>();

            for (i = 0; i < totalSize; i++)
            {
                int remainLength = totalSize - i;

                if (isGetbuffer)
                {
                    if (this.IsEndOfStream)
                    {
                        MapiInspector.MAPIParser.PartialGetType = type;
                        MapiInspector.MAPIParser.PartialGetRemainSize = remainLength;
                        MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                        MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                        break;
                    }
                }
                else if (isPutBuffer)
                {
                    if (this.IsEndOfStream)
                    {
                        MapiInspector.MAPIParser.PartialPutType = type;
                        MapiInspector.MAPIParser.PartialPutRemainSize = remainLength;
                        MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                        MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                        break;
                    }
                }
                else
                {
                    if (this.IsEndOfStream)
                    {
                        MapiInspector.MAPIParser.PartialPutExtendType = type;
                        MapiInspector.MAPIParser.PartialPutExtendRemainSize = remainLength;
                        MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                        MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                        break;
                    }
                }

                // fixedSizeValue is a split atom, so the blockSize will be read without split 
                l.Add(this.ReadBlock(blockSize));
            }

            return l.ToArray();
        }

        /// <summary>
        /// Read LengthOfBlock and advance the position.
        /// </summary>
        /// <param name="length">The length to read</param>
        /// <param name="type">The data type parsing</param>
        /// <param name="isGetbuffer">Check whether it's RopGetBuffer</param>
        /// <param name="isPutBuffer">Check whether it's RopPutBuffer</param>
        /// <returns>A LengthOfBlock specifies the length of the bytes array</returns>
        public LengthOfBlock ReadLengthBlockPartial(int length, ushort type, bool isGetbuffer, bool isPutBuffer)
        {
            int tmp = 0;

            if (isGetbuffer)
            {
                if (this.IsEndOfStream)
                {
                    MapiInspector.MAPIParser.PartialGetType = type;
                    MapiInspector.MAPIParser.PartialGetRemainSize = length;
                    MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }
                else
                {
                    if (MapiInspector.MAPIParser.PartialGetSubRemainSize != -1 && !this.IsEndOfStream
                        && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                        && MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                    {
                        tmp = MapiInspector.MAPIParser.PartialGetSubRemainSize;
                        MapiInspector.MAPIParser.PartialGetSubRemainSize = -1;
                        MapiInspector.MAPIParser.PartialGetServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialGetProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialGetClientInfo = string.Empty;
                    }
                    else
                    {
                        tmp = this.ReadInt32();
                    }

                    if (this.Length - this.Position < tmp)
                    {
                        MapiInspector.MAPIParser.PartialGetType = type;
                        MapiInspector.MAPIParser.PartialGetServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIParser.PartialGetProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                        MapiInspector.MAPIParser.PartialGetClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                        MapiInspector.MAPIParser.PartialGetSubRemainSize = tmp - (int)(this.Length - this.Position);
                        MapiInspector.MAPIParser.PartialGetRemainSize = length;
                        tmp = (int)(this.Length - this.Position);
                    }
                }
            }
            else if (isPutBuffer)
            {
                if (this.IsEndOfStream)
                {
                    MapiInspector.MAPIParser.PartialPutType = type;
                    MapiInspector.MAPIParser.PartialPutRemainSize = length;
                    MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }
                else
                {
                    if (MapiInspector.MAPIParser.PartialPutSubRemainSize != -1 && !this.IsEndOfStream && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                        && MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                    {
                        tmp = MapiInspector.MAPIParser.PartialPutSubRemainSize;
                        MapiInspector.MAPIParser.PartialPutSubRemainSize = -1;
                        MapiInspector.MAPIParser.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutClientInfo = string.Empty;
                    }
                    else
                    {
                        tmp = this.ReadInt32();
                    }

                    if (this.Length - this.Position < tmp)
                    {
                        MapiInspector.MAPIParser.PartialPutType = type;
                        MapiInspector.MAPIParser.PartialPutSubRemainSize = tmp - (int)(this.Length - this.Position);
                        tmp = (int)(this.Length - this.Position);
                        MapiInspector.MAPIParser.PartialPutRemainSize = length;
                        MapiInspector.MAPIParser.PartialPutServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIParser.PartialPutProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                        MapiInspector.MAPIParser.PartialPutClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                    }
                }
            }
            else
            {
                if (this.IsEndOfStream)
                {
                    MapiInspector.MAPIParser.PartialPutExtendType = type;
                    MapiInspector.MAPIParser.PartialPutExtendRemainSize = length;
                    MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                    MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                }
                else
                {
                    if (MapiInspector.MAPIParser.PartialPutExtendSubRemainSize != -1 && !this.IsEndOfStream && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                        && MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                    {
                        tmp = MapiInspector.MAPIParser.PartialPutExtendSubRemainSize;
                        MapiInspector.MAPIParser.PartialPutExtendSubRemainSize = -1;
                        MapiInspector.MAPIParser.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIParser.PartialPutExtendClientInfo = string.Empty;
                    }
                    else
                    {
                        tmp = this.ReadInt32();
                    }

                    if (this.Length - this.Position < tmp)
                    {
                        MapiInspector.MAPIParser.PartialPutExtendType = type;
                        MapiInspector.MAPIParser.PartialPutExtendSubRemainSize = tmp - (int)(this.Length - this.Position);
                        tmp = (int)(this.Length - this.Position);
                        MapiInspector.MAPIParser.PartialPutExtendRemainSize = length;
                        MapiInspector.MAPIParser.PartialPutExtendServerUrl = MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIParser.PartialPutExtendProcessName = MapiInspector.MAPIParser.ParsingSession.LocalProcess;
                        MapiInspector.MAPIParser.PartialPutExtendClientInfo = MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"];
                    }
                }
            }

            byte[] buffer = this.ReadBlock(tmp);
            return new LengthOfBlock(tmp, buffer);
        }

        /// <summary>
        /// Read a list of LengthOfBlock and advance the position.
        /// </summary>
        /// <param name="totalLength">The number of bytes to read</param>
        /// <param name="type">The data type parsing</param>
        /// <param name="isGetbuffer">Check whether it's RopGetBuffer</param>
        /// <param name="isPutBuffer">Check whether it's RopPutBuffer</param>
        /// <returns>A list of LengthOfBlock</returns>
        public LengthOfBlock[] ReadLengthBlocksPartial(int totalLength, ushort type, bool isGetbuffer, bool isPutBuffer)
        {
            int i = 0;
            List<LengthOfBlock> list = new List<LengthOfBlock>();

            while (i < totalLength)
            {
                int remainLength = totalLength - i;
                LengthOfBlock tmp = this.ReadLengthBlockPartial(remainLength, type, isGetbuffer, isPutBuffer);
                i += 1;
                list.Add(tmp);

                if (isGetbuffer)
                {
                    if ((MapiInspector.MAPIParser.PartialGetSubRemainSize != -1 || MapiInspector.MAPIParser.PartialGetRemainSize != -1)
                        && (MapiInspector.MAPIParser.PartialGetServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIParser.PartialGetProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                        && MapiInspector.MAPIParser.PartialGetClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"]))
                    {
                        break;
                    }
                }
                else if (isPutBuffer)
                {
                    if ((MapiInspector.MAPIParser.PartialPutSubRemainSize != -1 || MapiInspector.MAPIParser.PartialPutRemainSize != -1)
                        && (MapiInspector.MAPIParser.PartialPutServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIParser.PartialPutProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                        && MapiInspector.MAPIParser.PartialPutClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"]))
                    {
                        break;
                    }
                }
                else
                {
                    if ((MapiInspector.MAPIParser.PartialPutExtendSubRemainSize != -1 || MapiInspector.MAPIParser.PartialPutExtendRemainSize != -1)
                        && (MapiInspector.MAPIParser.PartialPutExtendServerUrl == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIParser.PartialPutExtendProcessName == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                        && MapiInspector.MAPIParser.PartialPutExtendClientInfo == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"]))
                    {
                        break;
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Get a UInt value and do not advance the position.
        /// </summary>
        /// <returns>A UInt value </returns>
        public uint VerifyUInt32()
        {
            try
            {
                return BitConverter.ToUInt32(
                    this.GetBuffer(),
                    (int)this.Position);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Get an unsigned short integer value for current position plus an offset and does not advance the position.
        /// </summary>
        /// <returns>An unsigned short integer value</returns>
        public ushort VerifyUInt16()
        {
            return BitConverter.ToUInt16(
                this.GetBuffer(),
                (int)this.Position);
        }

        /// <summary>
        /// Get an unsigned short integer value for current position plus an offset and do not advance the position.
        /// </summary>
        /// <param name="offset">An int value</param>
        /// <returns>An unsigned short integer value</returns>
        public ushort VerifyUInt16(int offset)
        {
            return BitConverter.ToUInt16(
                this.GetBuffer(),
                (int)this.Position + offset);
        }

        /// <summary>
        /// Indicate the Markers at the position equals a specified Markers.
        /// </summary>
        /// <param name="marker">A Markers value</param>
        /// <returns>True if the Markers at the position equals to the specified Markers, else false.</returns>
        public bool VerifyMarker(Markers marker)
        {
            return this.Verify((uint)marker);
        }

        /// <summary>
        /// Indicate the Markers at the current position plus an offset equals a specified Markers
        /// </summary>
        /// <param name="marker">A Markers to be verified</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the Markers at the current position plus an offset equals a specified Markers, else false.</returns>
        public bool VerifyMarker(Markers marker, int offset)
        {
            return this.Verify((uint)marker, offset);
        }

        /// <summary>
        /// Indicate the MetaProperties at the position equals a specified MetaProperties
        /// </summary>
        /// <param name="meta">A MetaProperties value</param>
        /// <returns>True if the MetaProperties at the position equals the specified MetaProperties, else false.</returns>
        public bool VerifyMetaProperty(MetaProperties meta)
        {
            return !this.IsEndOfStream && this.Verify((uint)meta, 0);
        }

        /// <summary>
        /// Indicate the UInt value at the position equals a specified UInt value.
        /// </summary>
        /// <param name="val">A UInt value.</param>
        /// <returns>True if the UInt at the position equals the specified uint.else false.</returns>
        public bool Verify(uint val)
        {
            return !this.IsEndOfStream && BitConverter.ToUInt32(
                this.GetBuffer(),
                (int)this.Position) == val;
        }

        /// <summary>
        /// Indicate the UInt value at the position plus an offset equals a specified UInt value.
        /// </summary>
        /// <param name="val">A UInt value</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the UInt at the position plus an offset equals the specified UInt,else false.</returns>
        public bool Verify(uint val, int offset)
        {
            return !this.IsEndOfStream && BitConverter.ToUInt32(
                this.GetBuffer(),
                (int)this.Position + offset) == val;
        }

        /// <summary>
        /// Indicate the byte value at the position plus an offset equals a specified byte
        /// </summary>
        /// <param name="val">A UInt value</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the byte at the position plus an offset equals the specified byte, else false.</returns>
        public bool Verify(byte val, int offset)
        {
            byte[] tmp = this.GetBuffer();
            return !this.IsEndOfStream && tmp[(int)this.Position + offset] == val;
        }
    }
}
