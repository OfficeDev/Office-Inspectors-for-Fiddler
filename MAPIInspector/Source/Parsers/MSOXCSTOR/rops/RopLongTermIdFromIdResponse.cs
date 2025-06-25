namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.8 RopLongTermIdFromId
    ///  A class indicates the RopLongTermIdFromId ROP Response Buffer.
    /// </summary>
    public class RopLongTermIdFromIdResponse : BaseStructure
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
        /// A LongTermID structure that specifies the long-term ID that was converted from the short-term ID, which is specified in the ObjectId field of the request.
        /// </summary>
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopLongTermIdFromIdResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopLongTermIdFromIdResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                LongTermId = new LongTermID();
                LongTermId.Parse(s);
            }
        }
    }
}
