namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  2.2.2.7 RopDeleteProperties
    ///  A class indicates the RopDeleteProperties ROP Request Buffer.
    /// </summary>
    public class RopDeletePropertiesRequest : BaseStructure
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
        /// An unsigned integer that specifies the number of PropertyTag structures in the PropertyTags field. 
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values to be deleted from the object. 
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopDeletePropertiesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeletePropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
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
