using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.8.6.1 RopSetProperties ROP Request Buffer
    /// A class indicates the RopSetProperties ROP Request Buffer.
    /// </summary>
    public class RopSetPropertiesRequest : Block
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
        /// An unsigned integer that specifies the number of PropertyValue structures listed in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specifies the property values to be set on the object.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Unparsed data left in the buffer.
        /// </summary>
        public BlockJunk Junk;

        /// <summary>
        /// Parse the RopSetPropertiesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            PropertyValueSize = ParseT<ushort>();
            PropertyValueCount = ParseT<ushort>();

            parser.PushCap(PropertyValueSize.Data - sizeof(ushort));

            var interValue = new List<TaggedPropertyValue>();
            for (int i = 0; i < PropertyValueCount && !parser.Empty; i++)
            {
                var value = new TaggedPropertyValue(PropertyCountContext.RopBuffers);
                value.Parse(parser);
                interValue.Add(value);
            }

            PropertyValues = interValue.ToArray();

            if (!parser.Empty && parser.RemainingBytes > 0)
            {
                // If there is still data left, grab it as a block
                Junk = ParseJunk("Remaining Data");
            }

            // Pop the cap to restore previous parsing limits
            parser.PopCap();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSetPropertiesRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(PropertyValueSize, "PropertyValueSize");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
            AddChild(Junk, "Junk");
        }
    }
}
