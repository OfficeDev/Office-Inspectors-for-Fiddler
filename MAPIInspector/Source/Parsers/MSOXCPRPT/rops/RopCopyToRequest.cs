namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.IO;

    /// <summary>
    ///  2.2.2.11 RopCopyTo
    ///  A class indicates the RopCopyTo ROP Request Buffer.
    /// </summary>
    public class RopCopyToRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored.
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored.
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether to copy subobjects.
        /// </summary>
        public bool WantSubObjects;

        /// <summary>
        /// A flags structure that contains flags that control the operation behavior.
        /// </summary>
        public CopyFlags CopyFlags;

        /// <summary>
        /// An unsigned integer that specifies how many tags are present in the ExcludedTags field.
        /// </summary>
        public ushort ExcludedTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to exclude from the copy. 
        /// </summary>
        public PropertyTag[] ExcludedTags;

        /// <summary>
        /// Parse the RopCopyToRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyToRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            SourceHandleIndex = ReadByte();
            DestHandleIndex = ReadByte();
            WantAsynchronous = ReadBoolean();
            WantSubObjects = ReadBoolean();
            CopyFlags = (CopyFlags)ReadByte();
            ExcludedTagCount = ReadUshort();
            ExcludedTags = new PropertyTag[(int)ExcludedTagCount];

            for (int i = 0; i < ExcludedTagCount; i++)
            {
                ExcludedTags[i] = Block.Parse<PropertyTag>(s);
            }
        }
    }
}
