using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.8 RopQueryPosition ROP
    /// The RopQueryPosition ROP ([MS-OXCROPS] section 2.2.5.7) returns the location of the cursor in the table.
    /// </summary>
    public class RopQueryPositionRequest : BaseStructure
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
        /// Parse the RopQueryPositionRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryPositionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
        }
    }
}
