using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetTemplateInfoRequest structure.
    /// 2.2.5.9 GetTemplateInfo
    /// </summary>
    public class GetTemplateInfoRequest : BaseStructure
    {
        /// <summary>
        /// A set of bit flags that specify options to the server.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// An unsigned integer that specifies the display type of the template for which information is requested.
        /// </summary>
        public uint DisplayType;

        /// <summary>
        /// A Boolean value that specifies whether the TemplateDn field is present.
        /// </summary>
        public bool HasTemplateDn;

        /// <summary>
        /// A null-terminated ASCII string that specifies the DN of the template requested.
        /// </summary>
        public MAPIString TemplateDn;

        /// <summary>
        /// An unsigned integer that specifies the code page of the template for which information is requested.
        /// </summary>
        public uint CodePage;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], of the template for which information is requested.
        /// </summary>
        public uint LocaleId;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetTemplateInfoRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetTemplateInfoRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            DisplayType = ReadUint();
            HasTemplateDn = ReadBoolean();

            if (HasTemplateDn)
            {
                TemplateDn = new MAPIString(Encoding.ASCII);
                TemplateDn.Parse(s);
            }

            CodePage = ReadUint();
            LocaleId = ReadUint();
            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}