using System.Collections.Generic;
using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.5 RopGetStoreState
    /// A class indicates the RopGetStoreState ROP Response Buffer.
    /// </summary>
    public class RopGetStoreStateResponse : Block
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
        /// A unsigned integer that indicates the state of the mailbox for the logged on user. 
        /// </summary>
        public BlockT<uint> StoreState;

        /// <summary>
        /// Parse the RopGetStoreStateResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                StoreState = ParseT<uint>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetStoreStateResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChild(StoreState, "StoreState");
        }
    }
}
