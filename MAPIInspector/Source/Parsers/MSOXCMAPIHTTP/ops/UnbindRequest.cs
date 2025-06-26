using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the UnbindRequest structure.
    /// 2.2.5 Request Types for Address Book Server Endpoint
    /// 2.2.5.2 Unbind
    /// </summary>
    public class UnbindRequest : BaseStructure
    {
        /// <summary>
        /// The reserved field
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Reserved = ReadUint();
            AuxiliaryBufferSize = ReadUint();
            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
            else
            {
                AuxiliaryBuffer = null;
            }
        }
    }
}