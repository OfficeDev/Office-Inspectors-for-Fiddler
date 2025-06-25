namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;

    /// <summary>
    ///  2.2.2.9 RopQueryNamedProperties
    ///  A class indicates the RopQueryNamedProperties ROP Request Buffer.
    /// </summary>
    public class RopQueryNamedPropertiesRequest : BaseStructure
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
        /// A flags structure that contains flags control how this ROP behaves.
        /// </summary>
        public byte QueryFlags;

        /// <summary>
        /// A Boolean that specifies whether the PropertyGuid field is present.
        /// </summary>
        public byte HasGuid;

        /// <summary>
        /// A GUID that is present if HasGuid is nonzero and is not present if the value of the HasGuid field is zero.
        /// </summary>
        public Guid? PropertyGuid;

        /// <summary>
        /// Parse the RopQueryNamedPropertiesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryNamedPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            QueryFlags = ReadByte();
            HasGuid = ReadByte();

            if (HasGuid != 0)
            {
                PropertyGuid = ReadGuid();
            }
        }
    }
}
