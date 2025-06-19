namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;

    /// <summary>
    /// Represents a REPLGUID and GLOBSET structure pair. 
    /// 2.2.2.4.2 Serialized IDSET Structure Containing a REPLGUID Structure
    /// </summary>
    public class IDSET_REPLGUID : Block
    {
        /// <summary>
        /// A GUID that identifies a REPLGUID structure. 
        /// </summary>
        public BlockT<Guid> REPLGUID;

        /// <summary>
        /// A serialized GLOBSET structure.
        /// </summary>
        public GLOBSET GLOBSET;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            REPLGUID = ParseT<Guid>();
            GLOBSET = Parse<GLOBSET>();
        }

        protected override void ParseBlocks()
        {
            SetText("IDSET_REPLGUID");
            AddChildBlockT(REPLGUID, "REPLGUID");
            AddChild(GLOBSET, "GLOBSET");
        }
    }
}
