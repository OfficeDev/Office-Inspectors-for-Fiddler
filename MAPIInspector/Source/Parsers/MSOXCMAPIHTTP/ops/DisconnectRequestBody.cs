using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the Disconnect request type.
    ///  2.2.4 Request Types for Mailbox Server Endpoint
    ///  2.2.4.3 Disconnect
    /// </summary>
    public class DisconnectRequestBody : BaseStructure
    {
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