using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.4 RopRestrict ROP
    /// The RopRestrict ROP ([MS-OXCROPS] section 2.2.5.3) establishes a restriction on a table. 
    /// </summary>
    public class RopRestrictRequest : BaseStructure
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
        /// A flags structure that contains flags that control this operation. 
        /// </summary>
        public AsynchronousFlags RestrictFlags;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public ushort RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this table The size of this field is specified by the RestrictionDataSize field.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// Parse the RopRestrictRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopRestrictRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            RestrictFlags = (AsynchronousFlags)ReadByte();
            RestrictionDataSize = ReadUshort();
            if (RestrictionDataSize > 0)
            {
                RestrictionType restriction = new RestrictionType();
                RestrictionData = restriction;
                RestrictionData.Parse(s);
            }
        }
    }
}
