namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.14 RopOpenStream
    ///  A class indicates the RopOpenStream ROP Response Buffer.
    /// </summary>
    public class RopOpenStreamResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that indicates the size of the stream opened.
        /// </summary>
        public uint? StreamSize;

        /// <summary>
        /// Parse the RopOpenStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            OutputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                StreamSize = ReadUint();
            }
        }
    }
}
