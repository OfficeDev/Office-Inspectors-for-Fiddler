namespace MAPIInspector.Parsers
{
    /// <summary>
    /// SizedXid structure.
    /// 2.2.2.3.1 SizedXid Structure
    /// </summary>
    public class SizedXid : BaseStructure
    {
        /// <summary>
        /// An unsigned 8-bit integer.
        /// </summary>
        public byte XidSize;

        /// <summary>
        /// A structure of type XID that contains the value of the internal identifier of an object, or internal or external identifier of a change number. 
        /// </summary>
        public XID Xid;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains SizedXid.</param>
        public void Parse(FastTransferStream stream)
        {
            this.XidSize = stream.ReadByte();
            this.Xid = new XID((int)this.XidSize);
            this.Xid.Parse(stream);
        }
    }
}
