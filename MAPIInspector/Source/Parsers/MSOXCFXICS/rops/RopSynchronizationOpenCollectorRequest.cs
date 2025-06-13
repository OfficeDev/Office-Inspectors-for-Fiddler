namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationOpenCollector ROP Request Buffer.
    ///  2.2.3.2.4.1.1 RopSynchronizationOpenCollector ROP Request Buffer
    /// </summary>
    public class RopSynchronizationOpenCollectorRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon
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
        /// A Boolean that specifies whether this synchronization upload context is for contents or for hierarchy.
        /// </summary>
        public bool IsContentsCollector;

        /// <summary>
        /// Parse the RopSynchronizationOpenCollectorRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationOpenCollectorRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.IsContentsCollector = this.ReadBoolean();
        }
    }
}
