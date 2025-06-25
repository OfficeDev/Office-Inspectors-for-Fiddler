namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.15 RopReadStream
    ///  A class indicates the RopReadStream ROP Request Buffer.
    /// </summary>
    public class RopReadStreamRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of bytes to read if the value is not equal to 0xBABE.
        /// </summary>
        public ushort ByteCount;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of bytes to read if the value of the ByteCount field is equal to 0xBABE.
        /// </summary>
        public uint MaximumByteCount;

        /// <summary>
        /// Parse the RopReadStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            ByteCount = ReadUshort();

            if (ByteCount == 0xBABE)
            {
                MaximumByteCount = ReadUint();
            }
        }
    }
}
