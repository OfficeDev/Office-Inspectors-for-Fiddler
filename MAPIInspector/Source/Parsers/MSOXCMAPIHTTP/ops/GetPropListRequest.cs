using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the GetPropListRequest structure.
    ///  2.2.5.6 GetPropList
    /// </summary>
    public class GetPropListRequest : BaseStructure
    {
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A MinimalEntryID structure that specifies the object for which to return properties.
        /// </summary>
        public MinimalEntryID MinimalId;

        /// <summary>
        /// An unsigned integer that specifies the code page that the server is being requested to use for string values of properties. 
        /// </summary>
        public uint CodePage;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropListRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetPropListRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            MinimalId = new MinimalEntryID();
            MinimalId.Parse(s);
            CodePage = ReadUint();
            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}