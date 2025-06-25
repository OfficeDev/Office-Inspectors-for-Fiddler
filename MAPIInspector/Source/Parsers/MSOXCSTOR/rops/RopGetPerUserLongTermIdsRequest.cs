namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;

    /// <summary>
    ///  2.2.1.10 RopGetPerUserLongTermIds
    ///  A class indicates the RopGetPerUserLongTermIds ROP Request Buffer.
    /// </summary>
    public class RopGetPerUserLongTermIdsRequest : BaseStructure
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
        /// A GUID that specifies which database the client is querying data for
        /// </summary>
        public Guid DatabaseGuid;

        /// <summary>
        /// Parse the RopGetPerUserLongTermIdsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPerUserLongTermIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            DatabaseGuid = ReadGuid();
        }
    }
}
