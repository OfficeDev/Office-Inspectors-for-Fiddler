using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represents an external identifier for an entity within a data store.
    /// 2.2.2.2 XID Structure
    /// </summary>
    public class XID : Block
    {
        /// <summary>
        /// A GUID that identifies the nameSpace that the identifier specified by LocalId belongs to
        public BlockGuid NamespaceGuid;

        /// <summary>
        /// A variable binary value that contains the ID of the entity in the nameSpace specified by NamespaceGuid.
        /// </summary>
        public BlockBytes LocalId;

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
        protected override void Parse()
        {
            NamespaceGuid = Parse<BlockGuid>();
            LocalId = ParseBytes(length - 16); // sizeof Guid is 16 bytes
        }

        protected override void ParseBlocks()
        {
            Text = "XID";
            this.AddChildGuid(NamespaceGuid, "NamespaceGuid");
            AddLabeledChild(LocalId, "LocalId");
        }
    }
}
