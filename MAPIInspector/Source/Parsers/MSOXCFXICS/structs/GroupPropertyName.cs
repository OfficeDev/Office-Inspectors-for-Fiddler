using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The GroupPropertyName.
    /// 2.2.2.8.1.1 GroupPropertyName
    /// </summary>
    public class GroupPropertyName : Block
    {
        /// <summary>
        /// The GUID that identifies the property set for the named property.
        /// </summary>
        public BlockGuid Guid;

        /// <summary>
        /// A value that identifies the type of property. 
        /// </summary>
        public BlockT<uint> Kind;

        /// <summary>
        /// A value that identifies the named property within its property set. 
        /// </summary>
        public BlockT<uint> Lid;

        /// <summary>
        /// A value that specifies the length of the Name field, in bytes. 
        /// </summary>
        public BlockT<uint> NameSize;

        /// <summary>
        /// A Unicode (UTF-16) string that identifies the property within the property set. 
        /// </summary>
        public PtypString Name;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        protected override void Parse()
        {
            Guid = Parse<BlockGuid>();
            Kind = ParseT<uint>();

            if (Kind == 0x00000000)
            {
                Lid = ParseT<uint>();
            }
            else if (Kind == 0x00000001)
            {
                NameSize = ParseT<uint>();
                Name = new PtypString((int)NameSize.Data);
                Name.Parse(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("GroupPropertyName");
            this.AddChildGuid(Guid, "Guid");
            AddChildBlockT(Kind, "Kind");
            AddChildBlockT(Lid, "Lid");
            AddChildBlockT(NameSize, "NameSize");
            AddChild(Name, "Name");
        }
    }
}