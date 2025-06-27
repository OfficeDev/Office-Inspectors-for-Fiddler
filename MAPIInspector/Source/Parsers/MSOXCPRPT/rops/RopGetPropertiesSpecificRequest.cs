using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  2.2.2.2 RopGetPropertiesSpecific
    ///  A class indicates the RopGetPropertiesSpecific ROP Request Buffer.
    /// </summary>
    public class RopGetPropertiesSpecificRequest : Block
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
        /// An unsigned integer that specifies the maximum size allowed for a property value returned.
        /// </summary>
        public BlockT<ushort> PropertySizeLimit;

        /// <summary>
        /// A Boolean that specifies whether to return string properties in multibyte Unicode.
        /// </summary>
        public BlockT<ushort> WantUnicode;

        /// <summary>
        /// An unsigned integer that specifies the number of tags present in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties requested.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopGetPropertiesSpecificRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            PropertySizeLimit = ParseT<ushort>();
            WantUnicode = ParseT<ushort>();
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
            SetText("RopGetPropertiesSpecificRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(PropertySizeLimit, "PropertySizeLimit");
            AddChildBlockT(WantUnicode, "WantUnicode");
            AddChildBlockT(PropertyTagCount, "PropertyTagCount");
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }
}
