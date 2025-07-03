using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.19 RopGetCollapseState ROP
    /// A class indicates the RopGetCollapseState ROP Response Buffer.
    /// </summary>
    public class RopGetCollapseStateResponse : BaseStructure
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
        /// An unsigned integer that specifies the size of the CollapseState field.
        /// </summary>
        public ushort? CollapseStateSize;

        /// <summary>
        /// An array of bytes that specifies a collapse state for a categorized table.
        /// </summary>
        public byte?[] CollapseState;

        /// <summary>
        /// Parse the RopGetCollapseStateResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetCollapseStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                CollapseStateSize = ReadUshort();
                CollapseState = ConvertArray(ReadBytes((int)CollapseStateSize));
            }
        }
    }
}
