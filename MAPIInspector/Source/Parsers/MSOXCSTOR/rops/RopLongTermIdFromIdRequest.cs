namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.8 RopLongTermIdFromId
    ///  A class indicates the RopLongTermIdFromId ROP Request Buffer.
    /// </summary>
    public class RopLongTermIdFromIdRequest : BaseStructure
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
        /// An identifier that specifies the short-term ID to be converted to a long-term ID.
        /// </summary>
        public byte[] ObjectId;

        /// <summary>
        /// Parse the RopLongTermIdFromIdRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopLongTermIdFromIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            ObjectId = ReadBytes(8);
        }
    }
}
