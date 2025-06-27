using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.11 RopSeekRowFractional ROP
    /// A class indicates the RopSeekRowFractional ROP Response Buffer.
    /// </summary>
    public class RopSeekRowFractionalResponse : BaseStructure
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
        /// Parse the RopSeekRowFractionalResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowFractionalResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
        }
    }
}
