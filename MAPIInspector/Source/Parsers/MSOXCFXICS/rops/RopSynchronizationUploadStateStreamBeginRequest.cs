namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamBegin ROP Request Buffer.
    ///  2.2.3.2.2.1.1 RopSynchronizationUploadStateStreamBegin ROP Request Buffer
    /// </summary>
    public class RopSynchronizationUploadStateStreamBeginRequest : BaseStructure
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
        /// A PropertyTag structure.
        /// </summary>
        public PropertyTag StateProperty;

        /// <summary>
        /// An unsigned integer that specifies the size of the stream to be uploaded.
        /// </summary>
        public uint TransferBufferSize;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamBeginRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamBeginRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.StateProperty = Block.Parse<PropertyTag>(s);

            this.TransferBufferSize = this.ReadUint();
        }
    }
}
