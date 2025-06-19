using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// SizedXid structure.
    /// 2.2.2.3.1 SizedXid Structure
    /// </summary>
    public class SizedXid : Block
    {
        /// <summary>
        /// An unsigned 8-bit integer.
        /// </summary>
        public BlockT<byte> XidSize;

        /// <summary>
        /// A structure of type XID that contains the value of the internal identifier of an object, or internal or external identifier of a change number. 
        /// </summary>
        public XID Xid;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            XidSize = ParseT<byte>();
            Xid = new XID(XidSize.Data);
            Xid.Parse(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("SizedXid");
            if (XidSize != null) AddChild(XidSize, $"XidSize:{XidSize.Data}");
            AddLabeledChild(Xid, "Xid");
        }
    }
}
