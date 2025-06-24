namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the Execute request type.
    ///  2.2.4 Request Types for Mailbox Server Endpoint
    ///  2.2.4.2 Execute
    /// </summary>
    public class ExecuteRequestBody : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specify to the server how to build the ROP responses in the RopBuffer field of the Execute request type success response body.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the RopBuffer field.
        /// </summary>
        public uint RopBufferSize;

        /// <summary>
        /// An structure of bytes that constitute the ROP request payload. 
        /// </summary>
        public RgbInputBuffer RopBuffer;

        /// <summary>
        /// An unsigned integer that specifies the maximum size for the RopBuffer field of the Execute request type success response body.
        /// </summary>
        public uint MaxRopOut;

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

            Flags = ReadUint();
            RopBufferSize = ReadUint();
            RopBuffer = new RgbInputBuffer(RopBufferSize);
            RopBuffer.Parse(s);
            MaxRopOut = ReadUint();
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