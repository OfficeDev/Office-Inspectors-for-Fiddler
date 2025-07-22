namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.2.4 Messaging Object EntryIDs Structures
    /// The enum of StoreObject type.
    /// </summary>
    public enum StoreObjectType : ushort
    {
        /// <summary>
        /// Private Folder (eitLTPrivateFolder) type
        /// </summary>
        PrivateFolder = 0x0001,

        /// <summary>
        /// PublicFolder (eitLTPublicFolder) type
        /// </summary>
        PublicFolder = 0x0003,

        /// <summary>
        /// MappedPublicFolder (eitLTWackyFolder) type
        /// </summary>
        MappedPublicFolder = 0x0005,

        /// <summary>
        /// PrivateMessage (eitLTPrivateMessage) type
        /// </summary>
        PrivateMessage = 0x0007,

        /// <summary>
        /// PublicMessage (eitLTPublicMessage) type
        /// </summary>
        PublicMessage = 0x0009,

        /// <summary>
        /// MappedPublicMessage (eitLTWackyMessage) type
        /// </summary>
        MappedPublicMessage = 0x000B,

        /// <summary>
        /// PublicNewsgroupFolder (eitLTPublicFolderByName) type
        /// </summary>
        PublicNewsgroupFolder = 0x000C
    }
}
