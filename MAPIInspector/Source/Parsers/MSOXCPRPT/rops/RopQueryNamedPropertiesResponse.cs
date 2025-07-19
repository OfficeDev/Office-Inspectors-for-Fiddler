using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.9 RopQueryNamedProperties
    /// A class indicates the RopQueryNamedProperties ROP Response Buffer.
    /// </summary>
    public class RopQueryNamedPropertiesResponse : Block
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
        /// An unsigned integer that specifies the number of elements contained in the PropertyIds and PropertyNames fields.
        /// </summary>
        public BlockT<ushort> IdCount;

        /// <summary>
        /// An array of unsigned 16-bit integers. Each integer in the array is the property ID associated with a property name.
        /// </summary>
        public BlockT<ushort>[] PropertyIds;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names for the property IDs specified in the PropertyIds field.
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopQueryNamedPropertiesResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                IdCount = ParseT<ushort>();
                var tmpPropertyIds = new List<BlockT<ushort>>();
                var tmpPropertyNames = new List<PropertyName>();

                for (int i = 0; i < IdCount; i++)
                {
                    tmpPropertyIds.Add(ParseT<ushort>());
                }
                PropertyIds = tmpPropertyIds.ToArray();

                for (int i = 0; i < IdCount; i++)
                {
                    tmpPropertyNames.Add(Parse<PropertyName>());
                }
                PropertyNames = tmpPropertyNames.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopQueryNamedPropertiesResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(IdCount, "IdCount");
            AddLabeledChildren(PropertyIds, "PropertyIds");
            AddLabeledChildren(PropertyNames, "PropertyNames");
        }
    }
}
