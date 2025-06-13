namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamEnd ROP Response Buffer.
    ///  2.2.3.2.2.3.2 RopSynchronizationUploadStateStreamEnd ROP Response Buffer
    /// </summary>
    public class RopSynchronizationUploadStateStreamEndResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamEndResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamEndResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
}
