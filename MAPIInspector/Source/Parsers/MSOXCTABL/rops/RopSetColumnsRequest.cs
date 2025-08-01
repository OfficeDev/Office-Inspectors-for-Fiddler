using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.5.1.1 RopSetColumns ROP Request Buffer
    /// The RopSetColumns ROP ([MS-OXCROPS] section 2.2.5.1) sets the properties that the client requests to be included in the table.
    /// </summary>
    public class RopSetColumnsRequest : Block
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
        /// A flags structure that contains flags that control this operation.
        /// </summary>
        public BlockT<AsynchronousFlags> SetColumnsFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of tags present in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that are visible in table rows.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopSetColumnsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            SetColumnsFlags = ParseT<AsynchronousFlags>();
            PropertyTagCount = ParseT<ushort>();

            var tempPropertyTags = new List<PropertyTag>();
            for (int i = 0; i < PropertyTagCount; i++)
            {
                tempPropertyTags.Add(Parse<PropertyTag>());
            }

            PropertyTags = tempPropertyTags.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSetColumnsRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(SetColumnsFlags, "SetColumnsFlags");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}
