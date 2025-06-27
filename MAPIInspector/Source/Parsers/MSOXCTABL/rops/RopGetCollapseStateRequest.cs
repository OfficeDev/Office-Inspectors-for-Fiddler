using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.19 RopGetCollapseState ROP
    /// The RopGetCollapseState ROP ([MS-OXCROPS] section 2.2.5.18) returns the data necessary to rebuild the current expanded/collapsed state of the table. 
    /// </summary>
    public class RopGetCollapseStateRequest : BaseStructure
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
        /// An identifier that specifies the row to be preserved as the cursor. 
        /// </summary>
        public long RowId;

        /// <summary>
        /// An unsigned integer that specifies the instance number of the row that is to be preserved as the cursor.
        /// </summary>
        public uint RowInstanceNumber;

        /// <summary>
        /// Parse the RopGetCollapseStateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetCollapseStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            RowId = ReadINT64();
            RowInstanceNumber = ReadUint();
        }
    }
}
