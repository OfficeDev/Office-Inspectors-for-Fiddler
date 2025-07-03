using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represents a REPLGUID and GLOBSET structure pair.
    /// 2.2.2.4.2 Serialized IDSET Structure Containing a REPLGUID Structure
    /// </summary>
    public class IDSET_REPLGUID : Block
    {
        /// <summary>
        /// A GUID that identifies a REPLGUID structure.
        /// </summary>
        public BlockGuid REPLGUID;

        /// <summary>
        /// A serialized GLOBSET structure.
        /// </summary>
        public GLOBSET GLOBSET;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            REPLGUID = Parse<BlockGuid>();
            GLOBSET = Parse<GLOBSET>();
        }

        protected override void ParseBlocks()
        {
            SetText("IDSET_REPLGUID");
            this.AddChildGuid(REPLGUID, "REPLGUID");
            AddChild(GLOBSET, "GLOBSET");
        }
    }
}
