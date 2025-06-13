namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyFolder ROP Request Buffer.
    ///  2.2.3.1.1.4.1 RopFastTransferSourceCopyFolder ROP Request Buffer
    /// </summary>
    public class RopFastTransferSourceCopyFolderRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation.
        /// </summary>
        public CopyFlags_CopyFolder CopyFlags;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.CopyFlags = (CopyFlags_CopyFolder)this.ReadByte();
            this.SendOptions = (SendOptions)this.ReadByte();
        }
    }
}
