using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.7.6.2 RopTransportSend ROP Success Response Buffer
    /// [MS-OXCROPS] 2.2.7.6.3 RopTransportSend ROP Failure Response Buffer
    /// /// A class indicates the RopTransportSend ROP Response Buffer.
    /// </summary>
    public class RopTransportSendResponse : Block
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
        /// A boolean that specifies whether property values are returned.
        /// </summary>
        public BlockT<byte> NoPropertiesReturned;

        /// <summary>
        /// An unsigned integer that specifies the number of structures returned in the PropertyValues field.
        /// </summary>
        public BlockT<ushort> PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specifies the properties to copy.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopTransportSendResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            //ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                NoPropertiesReturned = ParseT<byte>();
                PropertyValueCount = ParseT<ushort>();
                var tempPropertyValues = new List<TaggedPropertyValue>();

                for (int i = 0; i < PropertyValueCount; i++)
                {
                    var temptaggedPropertyValue = new TaggedPropertyValue(CountWideEnum.twoBytes);
                    temptaggedPropertyValue.Parse(parser);
                    tempPropertyValues.Add(temptaggedPropertyValue);
                }

                PropertyValues = tempPropertyValues.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopTransportSendResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChildBlockT(NoPropertiesReturned, "NoPropertiesReturned");
            AddChildBlockT(PropertyValueCount, "PropertyValueCount");
            AddLabeledChildren(PropertyValues, "PropertyValues");
        }
    }
}
