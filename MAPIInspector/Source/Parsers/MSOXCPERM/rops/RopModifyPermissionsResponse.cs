using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2 RopModifyPermissions ROP
    /// A class indicates the RopModifyPermissions ROP Response Buffer.
    /// </summary>
    public class RopModifyPermissionsResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x40.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// Parse the RopModifyPermissionsResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopModifyPermissionsResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
        }
    }
}
