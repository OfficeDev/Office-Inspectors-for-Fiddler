namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationOpenCollector ROP Response Buffer.
    ///  2.2.3.2.4.1.2 RopSynchronizationOpenCollector ROP Response Buffer
    /// </summary>
    public class RopSynchronizationOpenCollectorResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationOpenCollectorResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationOpenCollectorResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
}
