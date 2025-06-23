namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    /// <summary>
    /// 2.2.1.4 RopSetSearchCriteria ROP
    /// The RopSetSearchCriteria ROP ([MS-OXCROPS] section 2.2.4.4) establishes search criteria for a search folder.
    /// </summary>
    public class RopSetSearchCriteriaRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public BlockT<ushort> RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this search folder. 
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the FolderIds field.
        /// </summary>
        public BlockT<ushort> FolderIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which folders are searched. 
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        /// A flags structure that contains flags that control the search for a search folder.
        /// </summary>
        public BlockT<SearchRequestFlags> SearchFlags;

        /// <summary>
        /// Parse the RopSetSearchCriteriaRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            RestrictionDataSize = ParseT<ushort>();
            if (RestrictionDataSize.Data > 0)
            {
                RestrictionData = new RestrictionType();
                RestrictionData.Parse(parser);
            }

            FolderIdCount = ParseT<ushort>();
            var tempFolderIDs = new List<FolderID>();
            for (int i = 0; i < FolderIdCount.Data; i++)
            {
                tempFolderIDs.Add(Parse<FolderID>());
            }

            FolderIds = tempFolderIDs.ToArray();
            SearchFlags = ParseT<SearchRequestFlags>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetSearchCriteriaRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(RestrictionDataSize, "RestrictionDataSize");
            AddChild(RestrictionData);
            AddChildBlockT(FolderIdCount, "FolderIdCount");
            AddLabeledChildren(FolderIds, "FolderIds");
            AddChildBlockT(SearchFlags, "SearchFlags");
        }
    }
}