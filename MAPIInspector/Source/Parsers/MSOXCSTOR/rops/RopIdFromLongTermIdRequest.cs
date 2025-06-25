namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.9 RopIdFromLongTermId
    ///  A class indicates the RopIdFromLongTermId ROP Request Buffer.
    /// </summary>
    public class RopIdFromLongTermIdRequest : BaseStructure
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
        /// A LongTermID structure that specifies the long-term ID to be converted to a short-term ID.
        /// </summary>
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopIdFromLongTermIdRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopIdFromLongTermIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            LongTermId = new LongTermID();
            LongTermId.Parse(s);
        }
    }
}
