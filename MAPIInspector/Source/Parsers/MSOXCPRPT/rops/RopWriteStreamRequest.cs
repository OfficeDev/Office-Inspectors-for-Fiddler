namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.16 RopWriteStream
    ///  A class indicates the RopWriteStream ROP Request Buffer.
    /// </summary>
    public class RopWriteStreamRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the Data field.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An array of bytes that specifies the bytes to be written to the stream. The size of this field, in bytes, is specified by the DataSize field.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Parse the RopWriteStreamRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopWriteStreamRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            DataSize = ReadUshort();
            Data = ReadBytes((int)DataSize);
        }
    }
}
