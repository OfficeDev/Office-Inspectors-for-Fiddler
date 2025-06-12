namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represents a REPLID and GLOBSET structure pair. 
    /// 2.2.2.4.1 Serialized IDSET Structure Containing a REPLID Structure
    /// </summary>
    public class IDSET_REPLID : BaseStructure
    {
        /// <summary>
        /// A unsigned short which combined with all GLOBCNT structures contained in the GLOBSET field, produces a set of IDs.
        /// </summary>
        public ushort REPLID;

        /// <summary>
        /// A serialized GLOBSET structure.
        /// </summary>
        public GLOBSET GLOBSET;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains IDSET_REPLID.</param>
        public void Parse(FastTransferStream stream)
        {
            this.REPLID = stream.ReadUInt16();
            this.GLOBSET = new GLOBSET();
            this.GLOBSET.Parse(stream);
        }
    }
}
