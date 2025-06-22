namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 2.2.1.5 RopGetSearchCriteria ROP
    /// A class indicates the RopGetSearchCriteria ROP Response Buffer.
    /// </summary>
    public class RopGetSearchCriteriaResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public ushort? RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this search folder. 
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An unsigned integer that MUST be set to the value of the LogonId field in the request.
        /// </summary>
        public byte? LogonId;

        /// <summary>
        ///  An unsigned integer that specifies the number of identifiers in the FolderIds field.
        /// </summary>
        public ushort? FolderIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies which folders are searched. 
        /// </summary>
        public FolderID[] FolderIds;

        /// <summary>
        ///  A flags structure that contains flags that control the search for a search folder. 
        /// </summary>
        public SearchResponseFlags SearchFlags;

        /// <summary>
        /// Parse the RopGetSearchCriteriaResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetSearchCriteriaResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                RestrictionDataSize = ReadUshort();
                if (RestrictionDataSize > 0)
                {
                    RestrictionData = new RestrictionType();
                    RestrictionData.Parse(s);
                }

                LogonId = ReadByte();
                FolderIdCount = ReadUshort();
                List<FolderID> tempFolderIDs = new List<FolderID>();
                for (int i = 0; i < FolderIdCount; i++)
                {
                    FolderID folderID = new FolderID();
                    folderID.Parse(s);
                    tempFolderIDs.Add(folderID);
                }

                FolderIds = tempFolderIDs.ToArray();
                SearchFlags = (SearchResponseFlags)ReadUint();
            }
        }
    }
}