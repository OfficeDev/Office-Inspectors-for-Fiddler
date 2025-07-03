using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.18 RopCollapseRow ROP
    /// The RopCollapseRow ROP ([MS-OXCROPS] section 2.2.5.17) collapses an expanded category.
    /// </summary>
    public class RopCollapseRowRequest : BaseStructure
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
        /// An identifier that specifies the category to be collapsed.
        /// </summary>
        public long CategoryId;

        /// <summary>
        /// Parse the RopCollapseRowRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCollapseRowRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            CategoryId = ReadINT64();
        }
    }
}
