using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.10 RopGetPerUserLongTermIds
    /// A class indicates the RopGetPerUserLongTermIds ROP Response Buffer.
    /// </summary>
    public class RopGetPerUserLongTermIdsResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the LongTermIds field.
        /// </summary>
        public BlockT<ushort> LongTermIdCount;

        /// <summary>
        /// An array of LongTermID structures that specifies which folders the user has per-user information about.
        /// </summary>
        public LongTermID[] LongTermIds;

        /// <summary>
        /// Parse the RopGetPerUserLongTermIdsResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                LongTermIdCount = ParseT<ushort>();
                var tmpLongTermIds = new List<LongTermID>();
                for (int i = 0; i < LongTermIdCount; i++)
                {
                    tmpLongTermIds.Add(Parse<LongTermID>());
                }
                LongTermIds = tmpLongTermIds.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetPerUserLongTermIdsResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(LongTermIdCount, "LongTermIdCount");
            AddLabeledChildren(LongTermIds, "LongTermIds");
        }
    }
}
