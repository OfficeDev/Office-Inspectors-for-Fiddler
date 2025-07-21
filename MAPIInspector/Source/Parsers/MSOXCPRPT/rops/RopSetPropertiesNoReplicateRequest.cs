using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.6 RopSetPropertiesNoReplicate
    /// A class indicates the RopSetPropertiesNoReplicate ROP Request Buffer.
    /// </summary>
    public class RopSetPropertiesNoReplicateRequest : Block
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
        /// An unsigned integer that specifies the number of bytes used for the PropertyValueCount field and the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueSize;

        /// <summary>
        /// An unsigned integer that specifies the number of structures listed in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// PropertyValues (variable): An array of TaggedPropertyValue structures that specifies the property values to be set on the object.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSetPropertiesNoReplicateRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            PropertyValueSize = ParseT<ushort>();
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

        protected override void ParseBlocks()
        {
            Text = "RopSetPropertiesNoReplicateRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(PropertyValueSize, "PropertyValueSize");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
