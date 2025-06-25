namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.6 RopSetPropertiesNoReplicate
    ///  A class indicates the RopSetPropertiesNoReplicate ROP Request Buffer.
    /// </summary>
    public class RopSetPropertiesNoReplicateRequest : BaseStructure
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
        /// An unsigned integer that specifies the number of bytes used for the PropertyValueCount field and the PropertyValues field.
        /// </summary>
        public ushort PropertyValueSize;

        /// <summary>
        /// An unsigned integer that specifies the number of structures listed in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// PropertyValues (variable):  An array of TaggedPropertyValue structures that specifies the property values to be set on the object. 
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSetPropertiesNoReplicateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetPropertiesNoReplicateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            PropertyValueSize = ReadUshort();
            PropertyValueCount = ReadUshort();
            TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)PropertyValueCount];

            for (int i = 0; i < PropertyValueCount; i++)
            {
                interValue[i] = new TaggedPropertyValue();
                interValue[i].Parse(s);
            }

            PropertyValues = interValue;
        }
    }
}
