using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.7.2 RopAbortSubmit
    /// A class indicates the RopAbortSubmit ROP Request Buffer.
    /// </summary>
    public class RopAbortSubmitRequest : BaseStructure
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
        /// An identifier that identifies the folder in which the submitted message is located.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// An identifier that specifies the submitted message.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopAbortSubmitRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopAbortSubmitRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            FolderId = new FolderID();
            FolderId.Parse(s);
            MessageId = new MessageID();
            MessageId.Parse(s);
        }
    }
}
