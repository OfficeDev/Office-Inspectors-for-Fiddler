using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.12 RopGetPropertyIdsFromNames
    /// A class indicates the RopGetPropertyIdsFromNames ROP Response Buffer.
    /// </summary>
    public class RopGetPropertyIdsFromNamesResponse : Block
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
        /// An unsigned integer that specifies the number of integers contained in the PropertyIds field.
        /// </summary>
        public BlockT<ushort> PropertyIdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers. Each integer in the array is the property ID associated with a property name
        /// </summary>
        public BlockT<ushort>[] PropertyIds;

        /// <summary>
        /// Parse the RopGetPropertyIdsFromNamesResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                PropertyIdCount = ParseT<ushort>();
                var tmpPropertyIds = new List<BlockT<ushort>>();

                for (int i = 0; i < PropertyIdCount; i++)
                {
                    tmpPropertyIds.Add(ParseT<ushort>());
                }

                PropertyIds = tmpPropertyIds.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetPropertyIdsFromNamesResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(PropertyIdCount, "PropertyIdCount");
            AddLabeledChildren(PropertyIds, "PropertyIds");
        }
    }
}
