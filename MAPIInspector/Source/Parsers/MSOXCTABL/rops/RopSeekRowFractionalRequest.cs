using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.11 RopSeekRowFractional ROP
    /// The RopSeekRowFractional ROP ([MS-OXCROPS] section 2.2.5.10) moves the table cursor to an approximate position in the table.
    /// </summary>
    public class RopSeekRowFractionalRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that represents the numerator of the fraction identifying the table position to seek to.
        /// </summary>
        public uint Numerator;

        /// <summary>
        /// An unsigned integer that represents the denominator of the fraction identifying the table position to seek to.
        /// </summary>
        public uint Denominator;

        /// <summary>
        /// Parse the RopSeekRowFractionalRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowFractionalRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            Numerator = ReadUint();
            Denominator = ReadUint();
        }
    }
}
