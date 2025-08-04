using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.5.12.2 RopQueryColumnsAll ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.5.12.3 RopQueryColumnsAll ROP Failure Response Buffer
    /// A class indicates the RopQueryColumnsAll ROP Response Buffer.
    /// </summary>
    public class RopQueryColumnsAllResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
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
        /// An unsigned integer that specifies how many tags are present in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the columns of the table.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopQueryColumnsAllResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                PropertyTagCount = ParseT<ushort>();
                var tempPropertyTags = new List<PropertyTag>();
                for (int i = 0; i < PropertyTagCount; i++)
                {
                    tempPropertyTags.Add(Parse<PropertyTag>());
                }

                PropertyTags = tempPropertyTags.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopQueryColumnsAllResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}
