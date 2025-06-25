namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.19 RopGetStreamSize
    ///  A class indicates the RopGetStreamSize ROP Response Buffer.
    /// </summary>
    public class RopGetStreamSizeResponse : BaseStructure
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
        /// An unsigned integer that is the current size of the stream.
        /// </summary>
        public uint StreamSize;

        /// <summary>
        /// Parse the RopGetStreamSizeResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStreamSizeResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                StreamSize = ReadUint();
            }
        }
    }
}
