using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.7 RopGetStatus ROP
    /// The RopGetStatus ROP ([MS-OXCROPS] section 2.2.5.6) retrieves information about the current status of asynchronous operations being performed on the table.
    /// </summary>
    public class RopGetStatusRequest : BaseStructure
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
        /// Parse the RopGetStatusRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStatusRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
        }
    }
}
