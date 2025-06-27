using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.6 RopAbort ROP
    /// The RopAbort ROP ([MS-OXCROPS] section 2.2.5.5) attempts to stop any asynchronous table operations that are currently in progress
    /// </summary>
    public class RopAbortRequest : BaseStructure
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
        /// Parse the RopAbortRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopAbortRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
        }
    }
}
