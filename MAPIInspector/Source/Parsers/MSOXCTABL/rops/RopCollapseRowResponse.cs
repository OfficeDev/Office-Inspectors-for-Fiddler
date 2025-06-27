using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.18 RopCollapseRow ROP
    ///  A class indicates the RopCollapseRow ROP Response Buffer.
    /// </summary>
    public class RopCollapseRowResponse : BaseStructure
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
        /// An unsigned integer that specifies the total number of rows in the collapsed category.
        /// </summary>
        public uint? CollapsedRowCount;

        /// <summary>
        /// Parse the RopCollapseRowResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCollapseRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                CollapsedRowCount = ReadUint();
            }
        }
    }
}
