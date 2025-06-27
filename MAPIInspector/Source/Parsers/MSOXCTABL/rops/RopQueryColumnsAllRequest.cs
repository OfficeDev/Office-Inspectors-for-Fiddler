using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.13 RopQueryColumnsAll ROP
    /// The RopQueryColumnsAll ROP ([MS-OXCROPS] section 2.2.5.12) returns a complete list of all columns for the table. 
    /// </summary>
    public class RopQueryColumnsAllRequest : BaseStructure
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
        /// Parse the RopQueryColumnsAllRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryColumnsAllRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
        }
    }
}
