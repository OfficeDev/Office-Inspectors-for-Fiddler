namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.27 RopCloneStream
    ///  A class indicates the RopCloneStream ROP Response Buffer.
    /// </summary>
    public class RopCloneStreamResponse : BaseStructure
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
        /// Parse the RopCloneStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCloneStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
        }
    }
}
