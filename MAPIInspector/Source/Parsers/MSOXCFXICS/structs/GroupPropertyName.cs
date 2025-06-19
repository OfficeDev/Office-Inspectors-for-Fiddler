namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;

    /// <summary>
    /// The GroupPropertyName.
    /// 2.2.2.8.1.1 GroupPropertyName
    /// </summary>
    public class GroupPropertyName : Block
    {
        /// <summary>
        /// The GUID that identifies the property set for the named property.
        /// </summary>
        public BlockT<Guid> Guid;

        /// <summary>
        /// A value that identifies the type of property. 
        /// </summary>
        public BlockT<uint> Kind;

        /// <summary>
        ///  A value that identifies the named property within its property set. 
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
            Guid = ParseT<Guid>();
            Kind = ParseT<uint>();

            if (Kind.Data == 0x00000000)
            {
                Lid = ParseT<uint>();
            }
            else if (Kind.Data == 0x00000001)
            {
                NameSize = ParseT<uint>();
                Name = new PtypString((int)NameSize.Data);
                Name.Parse(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("GroupPropertyName");
            if (Guid != null) AddChild(Guid, $"Guid:{Guid.Data}");
            if (Kind != null) AddChild(Kind, $"Kind:{Kind.Data}");
            if (Lid != null) AddChild(Lid, $"Lid:{Lid.Data}");
            if (NameSize != null) AddChild(NameSize, $"NameSize:{NameSize.Data}");
            AddChild(Name, "Name");
        }
    }
}