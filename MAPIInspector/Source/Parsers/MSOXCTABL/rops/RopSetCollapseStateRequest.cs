using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.20 RopSetCollapseState ROP
    /// The following descriptions define valid fields for the RopSetCollapseState ROP request buffer ([MS-OXCROPS] section 2.2.5.19.1).
    /// </summary>
    public class RopSetCollapseStateRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the CollapseState field.
        /// </summary>
        public ushort CollapseStateSize;

        /// <summary>
        /// An array of bytes that specifies a collapse state for a categorized table. The size of this field, in bytes, is specified by the CollapseStateSize field.
        /// </summary>
        public byte[] CollapseState;

        /// <summary>
        /// Parse the RopSetCollapseStateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetCollapseStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            CollapseStateSize = ReadUshort();
            CollapseState = ReadBytes(CollapseStateSize);
        }
    }
}
