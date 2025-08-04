using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.8.8.1 RopDeleteProperties ROP Request Buffer
    /// A class indicates the RopDeleteProperties ROP Request Buffer.
    /// </summary>
    public class RopDeletePropertiesRequest : Block
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
        /// An unsigned integer that specifies the number of PropertyTag structures in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values to be deleted from the object.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopDeletePropertiesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            PropertyTagCount = ParseT<ushort>();
            List<PropertyTag> tmpPropertyTags = new List<PropertyTag>();

            for (int i = 0; i < PropertyTagCount; i++)
            {
                PropertyTag tmppropertytag = Parse<PropertyTag>();
                tmpPropertyTags.Add(tmppropertytag);
            }

            PropertyTags = tmpPropertyTags.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RopDeletePropertiesRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}
