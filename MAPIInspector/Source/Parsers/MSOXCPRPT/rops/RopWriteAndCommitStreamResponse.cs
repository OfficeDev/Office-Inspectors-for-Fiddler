namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.26 RopWriteAndCommitStream
    ///  A class indicates the RopWriteAndCommitStream ROP Response Buffer.
    /// </summary>
    public class RopWriteAndCommitStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of bytes actually written.
        /// </summary>
        public ushort WrittenSize;

        /// <summary>
        /// Parse the RopWriteAndCommitStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopWriteAndCommitStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            WrittenSize = ReadUshort();
        }
    }
}
