using BlockParser;
using System;
using System.Security.Cryptography;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The GroupPropertyName.
    /// [MS-OXCFXICS] 2.2.2.8.1.1 GroupPropertyName
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
        public BlockT<KindEnum> Kind;

        /// <summary>
        /// A value that identifies the named property within its property set.
        /// </summary>
        public BlockT<uint> Lid;

        /// <summary>
        /// A value that specifies the length of the Name field, in bytes.
        /// </summary>
        public BlockT<int> NameSize;

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
            Kind = ParseT<KindEnum>();

            if (Kind == KindEnum.LID)
            {
                Lid = ParseT<uint>();
            }
            else if (Kind == KindEnum.Name)
            {
                NameSize = ParseT<int>();
                Name = new PtypString(NameSize / 2);
                Name.Parse(parser);
            }
        }

        protected override void ParseBlocks()
        {
            Text = "GroupPropertyName";
            this.AddChildGuid(Guid, "Guid");
            AddChildBlockT(Kind, "Kind");

            NamedProperty namedProp = null;
            if (Guid != null && Lid != null)
            {
                namedProp = NamedProperty.Lookup(Guid.value, Lid);
            }

            if (Lid != null)
            {
                if (namedProp != null)
                    AddChild(Lid, $"Dispid: {namedProp.Name} = 0x{Lid:X4}");
                else
                    AddChild(Lid, $"Dispid: 0x{Lid:X4}");
            }

            AddChildBlockT(NameSize, "NameSize");
            AddChild(Name, "Name");
        }
    }
}
