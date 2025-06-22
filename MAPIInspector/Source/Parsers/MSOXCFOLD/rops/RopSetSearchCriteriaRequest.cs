namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.1.4 RopSetSearchCriteria ROP
    /// The RopSetSearchCriteria ROP ([MS-OXCROPS] section 2.2.4.4) establishes search criteria for a search folder.
    /// </summary>
    public class RopSetSearchCriteriaRequest : BaseStructure
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
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public ushort RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this search folder. 
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the FolderIds field.
        /// </summary>
        public ushort FolderIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which folders are searched. 
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        /// A flags structure that contains flags that control the search for a search folder.
        /// </summary>
        public SearchRequestFlags SearchFlags;

        /// <summary>
        /// Parse the RopSetSearchCriteriaRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetSearchCriteriaRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            RestrictionDataSize = ReadUshort();
            if (RestrictionDataSize > 0)
            {
                RestrictionData = new RestrictionType();
                RestrictionData.Parse(s);
            }

            FolderIdCount = ReadUshort();
            List<FolderID> tempFolderIDs = new List<FolderID>();
            for (int i = 0; i < FolderIdCount; i++)
            {
                FolderID folderID = new FolderID();
                folderID.Parse(s);
                tempFolderIDs.Add(folderID);
            }

            FolderIds = tempFolderIDs.ToArray();
            SearchFlags = (SearchRequestFlags)ReadUint();
        }
    }
}