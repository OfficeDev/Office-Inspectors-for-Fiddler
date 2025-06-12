namespace MAPIInspector.Parsers
{
    using System;

    #region 2.2.2.2 XID
    /// <summary>
    /// Represents an external identifier for an entity within a data store.
    /// </summary>
    public class XID : BaseStructure
    {
        /// <summary>
        /// A GUID that identifies the nameSpace that the identifier specified by LocalId belongs to
        /// </summary>
        public Guid NamespaceGuid;

        /// <summary>
        /// A variable binary value that contains the ID of the entity in the nameSpace specified by NamespaceGuid.
        /// </summary>
        public byte[] LocalId;

        /// <summary>
        /// A unsigned int value specifies the length of the LocalId.
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the XID class.
        /// </summary>
        /// <param name="length">the length of the LocalId.</param>
        public XID(int length)
        {
            this.length = length;
        }

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains XID.</param>
        public void Parse(FastTransferStream stream)
        {
            this.NamespaceGuid = stream.ReadGuid();
            this.LocalId = stream.ReadBlock((int)this.length - 16);
        }
    }

    #endregion
}
