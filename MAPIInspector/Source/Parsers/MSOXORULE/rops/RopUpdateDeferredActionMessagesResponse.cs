namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.3 RopUpdateDeferredActionMessages ROP
    /// A class indicates the RopUpdateDeferredActionMessages ROP Response Buffer.
    /// </summary>
    public class RopUpdateDeferredActionMessagesResponse : BaseStructure
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
        /// Parse the RopUpdateDeferredActionMessagesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopUpdateDeferredActionMessagesResponse structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
        }
    }
}
