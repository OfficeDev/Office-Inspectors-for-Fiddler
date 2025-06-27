using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.10 RopSeekRowBookmark ROP
    /// The RopSeekRowBookmark ROP ([MS-OXCROPS] section 2.2.5.9) moves the table cursor to a specific location in the table. 
    /// </summary>
    public class RopSeekRowBookmarkRequest : BaseStructure
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
        /// A signed integer that specifies the direction and the number of rows to seek.
        /// </summary>
        public int RowCount;

        /// <summary>
        /// A Boolean that specifies whether the server returns the actual number of rows sought in the response.
        /// </summary>
        public bool WantRowMovedCount;

        /// <summary>
        /// Parse the RopSeekRowBookmarkRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            BookmarkSize = ReadUshort();
            Bookmark = ReadBytes(BookmarkSize);
            RowCount = ReadINT32();
            WantRowMovedCount = ReadBoolean();
        }
    }
}
