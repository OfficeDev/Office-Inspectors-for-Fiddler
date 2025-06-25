namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.22 RopCopyToStream
    ///  A class indicates the RopCopyToStream ROP Request Buffer.
    /// </summary>
    public class RopCopyToStreamRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes to be copied.
        /// </summary>
        public ulong ByteCount;

        /// <summary>
        /// Parse the RopCopyToStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyToStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            SourceHandleIndex = ReadByte();
            DestHandleIndex = ReadByte();
            ByteCount = ReadUlong();
        }
    }
}
