using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.14 RopFindRow ROP
    /// The RopFindRow ROP ([MS-OXCROPS] section 2.2.5.13) returns the next row in a table that matches the search criteria and moves the cursor to that row.
    /// </summary>
    public class RopFindRowRequest : BaseStructure
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
        /// A flags structure that contains flags that control this operation.
        /// </summary>
        public FindRowFlags FindRowFlags;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public ushort RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this operation.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An enumeration that specifies where this operation begins its search.
        /// </summary>
        public Bookmarks Origin;

        /// <summary>
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public ushort BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the bookmark to use as the origin.
        /// </summary>
        public byte[] Bookmark;

        /// <summary>
        /// Parse the RopFindRow structure.
        /// </summary>
        /// <param name="s">A stream containing RopFindRow structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            FindRowFlags = (FindRowFlags)ReadByte();
            RestrictionDataSize = ReadUshort();
            if (RestrictionDataSize > 0)
            {
                RestrictionType tempRestriction = new RestrictionType();
                RestrictionData = tempRestriction;
                RestrictionData.Parse(s);
            }

            Origin = (Bookmarks)ReadByte();
            BookmarkSize = ReadUshort();
            Bookmark = ReadBytes(BookmarkSize);
        }
    }
}
