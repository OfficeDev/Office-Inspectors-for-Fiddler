using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.8 RopQueryPosition ROP
    /// A class indicates the  RopQueryPosition ROP Response Buffer.
    /// </summary>
    public class RopQueryPositionResponse : BaseStructure
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
        /// An unsigned integer that indicates the index (0-based) of the current row. 
        /// </summary>
        public uint? Numerator;

        /// <summary>
        /// An unsigned integer that indicates the total number of rows in the table. 
        /// </summary>
        public uint? Denominator;

        /// <summary>
        /// Parse the RopQueryPositionResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryPositionResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                Numerator = ReadUint();
                Denominator = ReadUint();
            }
        }
    }
}
