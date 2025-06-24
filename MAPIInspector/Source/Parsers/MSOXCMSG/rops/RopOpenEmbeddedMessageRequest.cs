namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.16 RopOpenEmbeddedMessage ROP
    /// A class indicates the RopOpenEmbeddedMessage ROP request Buffer.
    /// </summary>
    public class RopOpenEmbeddedMessageRequest : BaseStructure
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
        /// An identifier that specifies which code page is used for string values associated with the message.
        /// </summary>
        public ushort CodePageId;

        /// <summary>
        /// A flags structure that contains flags that control the access to the message.
        /// </summary>
        public OpenMessageModeFlags OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenEmbeddedMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenEmbeddedMessageRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.CodePageId = this.ReadUshort();
            this.OpenModeFlags = (OpenMessageModeFlags)this.ReadByte();
        }
    }
}
