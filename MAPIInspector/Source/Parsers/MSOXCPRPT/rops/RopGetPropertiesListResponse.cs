using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.8.5.2 RopGetPropertiesList ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.8.5.3 RopGetPropertiesList ROP Failure Response Buffer
    /// A class indicates the RopGetPropertiesList ROP Response Buffer.
    /// </summary>
    public class RopGetPropertiesListResponse : Block
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
        /// An unsigned integer that specifies the number of property tags in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that lists the property tags on the object.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopGetPropertiesListResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                PropertyTagCount = ParseT<ushort>();
                var tmpPropertyTags = new List<PropertyTag>();

                for (int i = 0; i < PropertyTagCount; i++)
                {
                    tmpPropertyTags.Add(Parse<PropertyTag>());
                }

                PropertyTags = tmpPropertyTags.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetPropertiesListResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}
