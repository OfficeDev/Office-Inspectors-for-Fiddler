namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.9 RopIdFromLongTermId
    ///  A class indicates the RopIdFromLongTermId ROP Response Buffer.
    /// </summary>
    public class RopIdFromLongTermIdResponse : BaseStructure
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
        /// An identifier that specifies the short-term ID that was converted from the long-term ID, which is specified in the LongTermId field of the request.
        /// </summary>
        public byte?[] ObjectId;

        /// <summary>
        /// Parse the RopIdFromLongTermIdResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopIdFromLongTermIdResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                ObjectId = ConvertArray(ReadBytes(8));
            }
        }
    }
}
