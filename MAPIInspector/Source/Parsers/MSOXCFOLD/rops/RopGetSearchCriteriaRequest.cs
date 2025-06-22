namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.1.5 RopGetSearchCriteria ROP
    /// The RopGetSearchCriteria ROP ([MS-OXCROPS] section 2.2.4.5) obtains the search criteria and the status of a search for a search folder. 
    /// </summary>
    public class RopGetSearchCriteriaRequest : BaseStructure
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
        /// A Boolean that specifies whether the client requests the restriction data (returned in the RestrictionData field of the response) to be specified with Unicode strings or with ASCII strings. 
        /// </summary>
        public bool UseUnicode;

        /// <summary>
        /// A Boolean that specifies whether the server includes the restriction information in the response.
        /// </summary>
        public bool IncludeRestriction;

        /// <summary>
        /// A Boolean that specifies whether the server includes the folders list in the response.
        /// </summary>
        public bool IncludeFolders;

        /// <summary>
        /// Parse the RopGetSearchCriteriaRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetSearchCriteriaRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            UseUnicode = ReadBoolean();
            IncludeRestriction = ReadBoolean();
            IncludeFolders = ReadBoolean();
        }
    }
}