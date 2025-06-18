using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represents a REPLID and GLOBSET structure pair. 
    /// 2.2.2.4.1 Serialized IDSET Structure Containing a REPLID Structure
    /// </summary>
    public class IDSET_REPLID : Block
    {
        /// <summary>
        /// A unsigned short which combined with all GLOBCNT structures contained in the GLOBSET field, produces a set of IDs.
        /// </summary>
        public BlockT<ushort> REPLID;

        /// <summary>
        /// A serialized GLOBSET structure.
        /// </summary>
        public GLOBSET GLOBSET;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            REPLID = ParseT<ushort>(parser);
            GLOBSET = Parse<GLOBSET>(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("IDSET_REPLID");
            if (REPLID != null) AddChild(REPLID, $"REPLID: {REPLID.Data} ({REPLID.Data:X4})");
            AddChild(GLOBSET, "GLOBSET");
        }
    }
}
