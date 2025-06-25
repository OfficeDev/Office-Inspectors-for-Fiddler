namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.IO;

    /// <summary>
    ///  2.2.2.10 RopCopyProperties
    ///  A class indicates the RopCopyProperties ROP Request Buffer.
    /// </summary>
    public class RopCopyPropertiesRequest : BaseStructure
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
        /// A flags structure that contains flags that control the operation behavior.
        /// </summary>
        public CopyFlags CopyFlags;

        /// <summary>
        /// An unsigned integer that specifies how many tags are present in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopCopyPropertiesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCopyPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            SourceHandleIndex = ReadByte();
            DestHandleIndex = ReadByte();
            WantAsynchronous = ReadBoolean();
            CopyFlags = (CopyFlags)ReadByte();
            PropertyTagCount = ReadUshort();
            PropertyTags = new PropertyTag[(int)PropertyTagCount];

            for (int i = 0; i < PropertyTagCount; i++)
            {
                PropertyTags[i] = Block.Parse<PropertyTag>(s);
            }
        }
    }
}
