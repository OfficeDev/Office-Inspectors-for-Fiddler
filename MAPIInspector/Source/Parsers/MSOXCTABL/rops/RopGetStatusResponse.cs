using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.7 RopGetStatus ROP
    /// A class indicates the RopGetStatus ROP Response Buffer.
    /// </summary>
    public class RopGetStatusResponse : BaseStructure
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
        /// An enumeration that specifies the status of the table.
        /// </summary>
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopGetStatusResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStatusResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                TableStatus = (TableStatus)ReadByte();
            }
        }
    }
}
