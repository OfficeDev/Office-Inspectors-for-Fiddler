using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFOLD] 2.2.1.5 RopGetSearchCriteria ROP
    /// The RopGetSearchCriteria ROP ([MS-OXCROPS] section 2.2.4.5) obtains the search criteria and the status of a search for a search folder.
    /// </summary>
    public class RopGetSearchCriteriaRequest : Block
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
        /// A Boolean that specifies whether the client requests the restriction data (returned in the RestrictionData field of the response) to be specified with Unicode strings or with ASCII strings.
        /// </summary>
        public BlockT<bool> UseUnicode;

        /// <summary>
        /// A Boolean that specifies whether the server includes the restriction information in the response.
        /// </summary>
        public BlockT<bool> IncludeRestriction;

        /// <summary>
        /// A Boolean that specifies whether the server includes the folders list in the response.
        /// </summary>
        public BlockT<bool> IncludeFolders;

        /// <summary>
        /// Parse the RopGetSearchCriteriaRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            UseUnicode = ParseAs<byte, bool>();
            IncludeRestriction = ParseAs<byte, bool>();
            IncludeFolders = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetSearchCriteriaRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(UseUnicode, "UseUnicode");
            AddChildBlockT(IncludeRestriction, "IncludeRestriction");
            AddChildBlockT(IncludeFolders, "IncludeFolders");
        }
    }
}
