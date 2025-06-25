namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  2.2.2.2 RopGetPropertiesSpecific
    ///  A class indicates the RopGetPropertiesSpecific ROP Request Buffer.
    /// </summary>
    public class RopGetPropertiesSpecificRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the maximum size allowed for a property value returned.
        /// </summary>
        public ushort PropertySizeLimit;

        /// <summary>
        /// A Boolean that specifies whether to return string properties in multibyte Unicode.
        /// </summary>
        public ushort WantUnicode;

        /// <summary>
        /// An unsigned integer that specifies the number of tags present in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties requested.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopGetPropertiesSpecificRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesSpecificRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            PropertySizeLimit = ReadUshort();
            WantUnicode = ReadUshort();
            PropertyTagCount = ReadUshort();
            List<PropertyTag> tmpPropertyTags = new List<PropertyTag>();

            for (int i = 0; i < PropertyTagCount; i++)
            {
                PropertyTag tmppropertytag = Block.Parse<PropertyTag>(s);
                tmpPropertyTags.Add(tmppropertytag);
            }

            PropertyTags = tmpPropertyTags.ToArray();
        }
    }
}
