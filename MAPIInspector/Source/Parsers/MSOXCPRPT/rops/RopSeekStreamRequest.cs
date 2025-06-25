namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.21 RopSeekStream
    ///  A class indicates the RopSeekStream ROP Request Buffer.
    /// </summary>
    public class RopSeekStreamRequest : BaseStructure
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
        /// An enumeration that specifies the origin location for the seek operation.
        /// </summary>
        public Origin Origin;

        /// <summary>
        /// An unsigned integer that specifies the seek offset.
        /// </summary>
        public ulong Offset;

        /// <summary>
        /// Parse the RopSeekStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            Origin = (Origin)ReadByte();
            Offset = ReadUlong();
        }
    }
}
