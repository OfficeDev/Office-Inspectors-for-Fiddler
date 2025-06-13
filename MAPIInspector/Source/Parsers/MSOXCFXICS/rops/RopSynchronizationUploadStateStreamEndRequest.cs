namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamEnd ROP Request Buffer.
    ///  2.2.3.2.2.3.1 RopSynchronizationUploadStateStreamEnd ROP Request Buffer
    /// </summary>
    public class RopSynchronizationUploadStateStreamEndRequest : BaseStructure
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
        /// Parse the RopSynchronizationUploadStateStreamEndRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamEndRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }
}
