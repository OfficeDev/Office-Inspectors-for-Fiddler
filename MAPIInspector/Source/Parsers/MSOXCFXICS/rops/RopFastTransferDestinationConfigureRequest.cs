namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationConfigure ROP Request Buffer.
    ///  2.2.3.1.2.1.1 RopFastTransferDestinationConfigure ROP Request Buffer
    /// </summary>
    public class RopFastTransferDestinationConfigureRequest : BaseStructure
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
        /// An enumeration that indicates how the data stream was created on the source.
        /// </summary>
        public SourceOperation SourceOperation;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the transfer operation.
        /// </summary>
        public CopyFlags_DestinationConfigure CopyFlags;

        /// <summary>
        /// Parse the RopFastTransferDestinationConfigureRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationConfigureRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.SourceOperation = (SourceOperation)this.ReadByte();
            this.CopyFlags = (CopyFlags_DestinationConfigure)this.ReadByte();
        }
    }
}
