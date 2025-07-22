using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.3 RopGetPropertiesAll
    /// A class indicates the RopGetPropertiesAll ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesAllResponse : Block
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
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that are the properties defined on the object.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopGetPropertiesAllResponse structure.
        /// </summary>
        protected override void Parse()
        {

            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                PropertyValueCount = ParseT<ushort>();
                var interValue = new List<TaggedPropertyValue>();
                for (int i = 0; i < PropertyValueCount; i++)
                {
                    var value = new TaggedPropertyValue();
                    value.Parse(parser);
                    interValue.Add(value);
                }

                PropertyValues = interValue.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetPropertiesAllResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ErrorCode ");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
