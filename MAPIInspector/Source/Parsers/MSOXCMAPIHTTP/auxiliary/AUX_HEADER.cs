using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The AUX_HEADER structure provides information about the auxiliary block structures that follow it. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// </summary>
    public class AUX_HEADER : Block
    {
        /// <summary>
        /// The size of the AUX_HEADER structure plus any additional payload data.
        /// </summary>
        public BlockT<ushort> _Size;

        /// <summary>
        /// The version information of the payload data.
        /// </summary>
        public BlockT<PayloadDataVersion> Version;

        /// <summary>
        /// The type of auxiliary block data structure. The Type should be AuxiliaryBlockType_1 or AuxiliaryBlockType_2.
        /// </summary>
        public BlockT<AuxiliaryBlockType_1> Type1;
        public BlockT<AuxiliaryBlockType_2> Type2;

        /// <summary>
        /// Parse the AUX_HEADER structure.
        /// </summary>
        protected override void Parse()
        {
            _Size = ParseT<ushort>();
            Version = ParseT<PayloadDataVersion>();

            if (Version == PayloadDataVersion.AUX_VERSION_1)
            {
                Type1 = ParseT<AuxiliaryBlockType_1>();
            }
            else
            {
                Type2 = ParseT<AuxiliaryBlockType_2>();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("AUX_HEADER");
            AddChildBlockT(_Size, "Size");
            AddChildBlockT(Version, "Version");
            AddChildBlockT(Type1, "Type");
            AddChildBlockT(Type2, "Type");
        }
    }
}