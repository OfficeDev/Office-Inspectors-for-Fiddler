namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// Represents a REPLGUID and GLOBSET structure pair. 
    /// 2.2.2.4.2 Serialized IDSET Structure Containing a REPLGUID Structure
    /// </summary>
    public class IDSET_REPLGUID : BaseStructure
    {
        /// <summary>
        /// A GUID that identifies a REPLGUID structure. 
        /// </summary>
        public Guid REPLGUID;

        /// <summary>
        /// A serialized GLOBSET structure.
        /// </summary>
        public GLOBSET GLOBSET;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains IDSET_REPLGUID.</param>
        public void Parse(FastTransferStream stream)
        {
            this.REPLGUID = stream.ReadGuid();
            this.GLOBSET = new GLOBSET();
            this.GLOBSET.Parse(stream);
        }
    }
}
