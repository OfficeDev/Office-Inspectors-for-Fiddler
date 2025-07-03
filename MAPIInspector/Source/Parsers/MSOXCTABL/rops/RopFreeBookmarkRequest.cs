using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.15 RopFreeBookmark ROP
    /// The RopFreeBookmark ROP ([MS-OXCROPS] section 2.2.5.14) frees the memory associated with a bookmark that was returned by a previous RopCreateBookmark ROP request ([MS-OXCROPS] section 2.2.5.11).
    /// </summary>
    public class RopFreeBookmarkRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public ushort BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the origin for the seek operation.
        /// </summary>
        public byte[] Bookmark;

        /// <summary>
        /// Parse the RopFreeBookmarkRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFreeBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            BookmarkSize = ReadUshort();
            Bookmark = ReadBytes(BookmarkSize);
        }
    }
}
