namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.20 RopSetStreamSize
    ///  A class indicates the RopSetStreamSize ROP Request Buffer.
    /// </summary>
    public class RopSetStreamSizeRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the stream.
        /// </summary>
        public ulong StreamSize;

        /// <summary>
        /// Parse the RopSetStreamSizeRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetStreamSizeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            StreamSize = ReadUlong();
        }
    }
}
