using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.7.7 RopTransportNewMail
    /// A class indicates the RopTransportNewMail ROP Request Buffer.
    /// </summary>
    public class RopTransportNewMailRequest : BaseStructure
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
        /// An identifier that specifies the new message object.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// An identifier that identifies the folder of the new message object.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies the message class of the new message object;
        /// </summary>
        public MAPIString MessageClass;

        /// <summary>
        /// A flags structure that contains the message flags of the new message object.
        /// </summary>
        public MessageFlags MessageFlags;

        /// <summary>
        /// Parse the RopTransportNewMailRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopTransportNewMailRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            MessageId = new MessageID();
            MessageId.Parse(s);
            FolderId = new FolderID();
            FolderId.Parse(s);
            MessageClass = new MAPIString(Encoding.ASCII);
            MessageClass.Parse(s);
            MessageFlags = (MessageFlags)ReadUint();
        }
    }
}
