using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ROP output buffer, which is sent by the server, includes an array of ROP response buffers.
    /// </summary>
    public class ROPInputBuffer_WithoutCROPS : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the size of both this field and the RopsList field.
        /// </summary>
        public BlockT<ushort> RopSize;

        /// <summary>
        /// An array of ROP request buffers.
        /// </summary>
        public BlockBytes RopsList;

        /// <summary>
        /// An array of 32-bit values. Each 32-bit value specifies a Server object handle that is referenced by a ROP buffer.
        /// </summary>
        public BlockT<uint>[] ServerObjectHandleTable;
        public List<uint> ServerObjectHandleTableList
        {
            get
            {
                var list = new List<uint>();
                if (ServerObjectHandleTable != null)
                {
                    foreach (var handle in ServerObjectHandleTable)
                    {
                        list.Add(handle.Data);
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// Parse the ROPInputBuffer_WithoutCROPS structure.
        /// </summary>
        protected override void Parse()
        {
            RopSize = ParseT<ushort>();
            RopsList = ParseBytes(RopSize - 2);

            var serverObjectHandleTable = new List<BlockT<uint>>();
            while (parser.RemainingBytes > sizeof(uint))
            {
                serverObjectHandleTable.Add(ParseT<uint>());
            }

            ServerObjectHandleTable = serverObjectHandleTable.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("ROPInputBuffer_WithoutCROPS");
            AddChildBlockT(RopSize, "RopSize");
            AddChildBytes(RopsList, "RopsList");
            AddLabeledChildren(ServerObjectHandleTable, "ServerObjectHandleTable");
        }
    }
}
