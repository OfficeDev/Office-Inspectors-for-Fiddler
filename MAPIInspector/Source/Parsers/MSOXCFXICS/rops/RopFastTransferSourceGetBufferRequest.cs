namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferSourceGetBuffer ROP Request Buffer.
    ///  2.2.3.1.1.5.1 RopFastTransferSourceGetBuffer ROP Request Buffer
    /// </summary>
    public class RopFastTransferSourceGetBufferRequest : BaseStructure
    {
        /// <summary>
        /// A byte that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// A byte that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// A byte that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An UShort that specifies the buffer size requested.
        /// </summary>
        public ushort BufferSize;

        /// <summary>
        /// An UShort that is present when the BufferSize field is set to 0xBABE.
        /// </summary>
        public ushort? MaximumBufferSize;

        /// <summary>
        /// Parse the RopFastTransferSourceGetBufferRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceGetBufferRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.BufferSize = this.ReadUshort();
            if (this.BufferSize == 0xBABE)
            {
                this.MaximumBufferSize = this.ReadUshort();
            }
        }
    }
}
