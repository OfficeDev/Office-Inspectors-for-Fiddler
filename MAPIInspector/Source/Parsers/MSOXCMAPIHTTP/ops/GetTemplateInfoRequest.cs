using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetTemplateInfoRequest structure.
    /// 2.2.5.9 GetTemplateInfo
    /// </summary>
    public class GetTemplateInfoRequest : Block
    {
        /// <summary>
        /// A set of bit flags that specify options to the server.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// An unsigned integer that specifies the display type of the template for which information is requested.
        /// </summary>
        public BlockT<uint> DisplayType;

        /// <summary>
        /// A Boolean value that specifies whether the TemplateDn field is present.
        /// </summary>
        public BlockT<bool> HasTemplateDn;

        /// <summary>
        /// A null-terminated ASCII string that specifies the DN of the template requested.
        /// </summary>
        public BlockString TemplateDn;

        /// <summary>
        /// An unsigned integer that specifies the code page of the template for which information is requested.
        /// </summary>
        public BlockT<uint> CodePage;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], of the template for which information is requested.
        /// </summary>
        public BlockT<uint> LocaleId;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetTemplateInfoRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            DisplayType = ParseT<uint>();
            HasTemplateDn = ParseAs<byte, bool>();
            if (HasTemplateDn) TemplateDn = ParseStringA();
            CodePage = ParseT<uint>();
            LocaleId = ParseT<uint>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "GetTemplateInfoRequest";
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(DisplayType, "DisplayType");
            AddChildBlockT(HasTemplateDn, "HasTemplateDn");
            AddChild(TemplateDn, "TemplateDn");
            AddChildBlockT(CodePage, "CodePage");
            AddChildBlockT(LocaleId, "LocaleId");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }

    }
}