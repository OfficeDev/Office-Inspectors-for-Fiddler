using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Bind request type request body.
    /// 2.2.5 Request Types for Address Book Server Endpoint
    /// 2.2.5.1 Bind
    /// </summary>
    public class BindRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public byte HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

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
            HasState = ReadByte();

            if (HasState != 0)
            {
                State = new STAT();
                State.Parse(s);
            }
            else
            {
                State = null;
            }

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