using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the CompareMinIdsRequest structure.
    ///  2.2.5 Request Types for Address Book Server Endpoint 
    ///  2.2.5.3 CompareMinIds
    /// </summary>
    public class CompareMinIdsRequest : BaseStructure
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field. 
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public byte HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A MinimalEntryID structure ([MS-OXNSPI] section 2.2.9.1) that specifies the Minimal Entry ID of the first object.
        /// </summary>
        public MinimalEntryID MinimalId1;

        /// <summary>
        /// A MinimalEntryID structure that specifies the Minimal Entry ID of the second object.
        /// </summary>
        public MinimalEntryID MinimalId2;

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

            MinimalId1 = new MinimalEntryID();
            MinimalId1.Parse(s);
            MinimalId2 = new MinimalEntryID();
            MinimalId2.Parse(s);
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