using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the QueryColumnsRequest structure.
    /// 2.2.5.13 QueryColumns
    /// </summary>
    public class QueryColumnsRequest : BaseStructure
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A set of bit flags that specify options to the server.
        /// </summary>
        public uint MapiFlags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryColumnsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing QueryColumnsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Reserved = ReadUint();
            MapiFlags = ReadUint();
            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}