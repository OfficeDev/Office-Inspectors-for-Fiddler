using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the RopSetLocalReplicaMidsetDeleted ROP Request Buffer.
    ///  2.2.13.12.1 RopSetLocalReplicaMidsetDeleted ROP Request Buffer
    /// </summary>
    public class RopSetLocalReplicaMidsetDeletedRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of both the LongTermIdRangeCount and LongTermIdRanges fields.
        /// </summary>
        public BlockT<ushort> DataSize;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the LongTermIdRanges field.
        /// </summary>
        public BlockT<uint> LongTermIdRangeCount;

        /// <summary>
        /// An array of LongTermIdRange structures that specify the ranges of message identifiers that have been deleted.
        /// </summary>
        public LongTermIdRange[] LongTermIdRanges;

        /// <summary>
        /// Parse the RopSetLocalReplicaMidsetDeletedRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            DataSize = ParseT<ushort>();
            LongTermIdRangeCount = ParseT<uint>();

            var interRangs = new List<LongTermIdRange>();
            for (int i = 0; i < LongTermIdRangeCount; i++)
            {
                interRangs.Add(Parse<LongTermIdRange>());
            }

            LongTermIdRanges = interRangs.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetLocalReplicaMidsetDeletedRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(DataSize, "DataSize");
            AddChildBlockT(LongTermIdRangeCount, "LongTermIdRangeCount");
            AddLabeledChildren(LongTermIdRanges, "LongTermIdRanges");
        }
    }
}
