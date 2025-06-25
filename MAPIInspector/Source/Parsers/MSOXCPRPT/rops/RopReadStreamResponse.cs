namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.15 RopReadStream
    ///  A class indicates the RopReadStream ROP Response Buffer.
    /// </summary>
    public class RopReadStreamResponse : BaseStructure
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
        /// An unsigned integer that specifies the size, in bytes, of the Data field.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An array of bytes that are the bytes read from the stream.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the RopReadStreamResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadStreamResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            DataSize = ReadUshort();
            Data = ReadBytes((int)DataSize);
        }
    }
}
