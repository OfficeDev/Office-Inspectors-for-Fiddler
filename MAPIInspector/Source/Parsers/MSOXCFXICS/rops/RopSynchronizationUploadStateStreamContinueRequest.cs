namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamContinue ROP Request Buffer.
    ///  2.2.3.2.2.2.1 RopSynchronizationUploadStateStreamContinue ROP Request Buffer
    /// </summary>
    public class RopSynchronizationUploadStateStreamContinueRequest : BaseStructure
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
        /// An unsigned integer that specifies the size, in bytes, of the StreamData field.
        /// </summary>
        public uint StreamDataSize;

        /// <summary>
        /// An array of bytes that contains the state stream data to be uploaded.
        /// </summary>
        public byte[] StreamData;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamContinueRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamContinueRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.StreamDataSize = this.ReadUint();
            this.StreamData = this.ReadBytes((int)this.StreamDataSize);
        }
    }
}
