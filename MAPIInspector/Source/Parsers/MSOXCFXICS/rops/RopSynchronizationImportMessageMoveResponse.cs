namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageMove ROP Response Buffer.
    ///  2.2.3.2.4.4.2 RopSynchronizationImportMessageMove ROP Response Buffer
    /// </summary>
    public class RopSynchronizationImportMessageMoveResponse : BaseStructure
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
        /// An identifier.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageMoveResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportMessageMoveResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
            }
        }
    }
}
