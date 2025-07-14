using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Lightweight parsing of a ROP input or output buffer to get the server object handles.
    /// </summary>
    public class ROPBufferServerObjectTable : Block
    {
        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public List<uint> ServerObjectHandleTable;

        /// <summary>
        /// Parse the ROPBufferServerObjectTable structure.
        /// </summary>
        protected override void Parse()
        {
            var ropSize = ReadT<ushort>();
            if (ropSize <= sizeof(ushort)) return;
            parser.Advance(ropSize - sizeof(ushort));
            ServerObjectHandleTable = new List<uint>();
            while (parser.RemainingBytes >= sizeof(uint))
            {
                ServerObjectHandleTable.Add(ParseT<uint>());
            }
        }

        protected override void ParseBlocks() { }
    }
}
