namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.12 RopGetPropertyIdsFromNames
    ///  A class indicates the RopGetPropertyIdsFromNames ROP Request Buffer.
    /// </summary>
    public class RopGetPropertyIdsFromNamesRequest : BaseStructure
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
        /// An integer that specifies whether to create a new entry.
        /// </summary>
        public byte Flags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyNames field.
        /// </summary>
        public ushort PropertyNameCount;

        /// <summary>
        /// A list of PropertyName structures that specifies the property names requested.
        /// </summary>
        public PropertyName[] PropertyNames;

        /// <summary>
        /// Parse the RopGetPropertyIdsFromNamesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertyIdsFromNamesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            Flags = ReadByte();
            PropertyNameCount = ReadUshort();
            PropertyNames = new PropertyName[(int)PropertyNameCount];

            for (int i = 0; i < PropertyNameCount; i++)
            {
                PropertyNames[i] = new PropertyName();
                PropertyNames[i].Parse(s);
            }
        }
    }
}
