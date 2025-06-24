namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.17 RopGetAttachmentTable ROP
    /// A class indicates the RopGetAttachmentTable ROP request Buffer.
    /// </summary>
    public class RopGetAttachmentTableRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. 
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the type of table. 
        /// </summary>
        public GetAttachmentTableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetAttachmentTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetAttachmentTableRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            OutputHandleIndex = ReadByte();
            TableFlags = (GetAttachmentTableFlags)ReadByte();
        }
    }
}
