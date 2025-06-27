using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.12 RopCreateBookmark ROP
    /// The RopCreateBookmark ROP ([MS-OXCROPS] section 2.2.5.11) creates a new bookmark at the current cursor position in the table. 
    /// </summary>
    public class RopCreateBookmarkRequest : BaseStructure
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
        /// Parse the RopCreateBookmarkRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
        }
    }
}
