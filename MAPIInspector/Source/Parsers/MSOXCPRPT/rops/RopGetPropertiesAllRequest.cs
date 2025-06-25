namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.2.3 RopGetPropertiesAll
    ///  A class indicates the RopGetPropertiesAll ROP Request Buffer.
    /// </summary>
    public class RopGetPropertiesAllRequest : BaseStructure
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
        /// Parse the RopGetPropertiesAllRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetPropertiesAllRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            PropertySizeLimit = ReadUshort();
            WantUnicode = ReadUshort();
        }
    }
}
