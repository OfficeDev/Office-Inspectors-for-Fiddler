using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.5 RopGetSearchCriteria ROP
    /// A class indicates the RopGetSearchCriteria ROP Response Buffer.
    /// </summary>
    public class RopGetSearchCriteriaResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public BlockT<ushort> RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this search folder.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An unsigned integer that MUST be set to the value of the LogonId field in the request.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        ///  An unsigned integer that specifies the number of identifiers in the FolderIds field.
        /// </summary>
        public BlockT<ushort> FolderIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which folders are searched.
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        /// A flags structure that contains flags that control the search for a search folder.
        /// </summary>
        public BlockT<SearchResponseFlags> SearchFlags;

        /// <summary>
        /// Parse the RopGetSearchCriteriaResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                RestrictionDataSize = ParseT<ushort>();
                if (RestrictionDataSize > 0)
                {
                    RestrictionData = new RestrictionType();
                    RestrictionData.Parse(parser);
                }

                LogonId = ParseT<byte>();
                FolderIdCount = ParseT<ushort>();
                var tempFolderIDs = new List<FolderID>();
                for (int i = 0; i < FolderIdCount; i++)
                {
                    tempFolderIDs.Add(Parse<FolderID>());
                }

                FolderIds = tempFolderIDs.ToArray();
                SearchFlags = ParseT<SearchResponseFlags>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetSearchCriteriaResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(RestrictionDataSize, "RestrictionDataSize");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(FolderIdCount, "FolderIdCount");
            AddLabeledChildren(FolderIds, "FolderIds");
            AddChildBlockT(SearchFlags, "SearchFlags");
        }
    }
}