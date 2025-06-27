using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ResortRestrictionRequest structure.
    /// 2.2.5.15 ResortRestriction
    /// </summary>
    public class ResortRestrictionRequest : BaseStructure
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public bool HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the MinimalIds field. 
        /// </summary>
        public uint MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures that compose a restricted address book container. 
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResortRestrictionRequest structure.
        /// </summary>
        /// <param name="s">A stream containing ResortRestrictionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Reserved = ReadUint();
            HasState = ReadBoolean();

            if (HasState)
            {
                State = new STAT();
                State.Parse(s);
            }

            HasMinimalIds = ReadBoolean();

            if (HasMinimalIds)
            {
                MinimalIdCount = ReadUint();
                List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();

                for (int i = 0; i < MinimalIdCount; i++)
                {
                    MinimalEntryID miniEID = new MinimalEntryID();
                    miniEID.Parse(s);
                    miniEIDList.Add(miniEID);
                }

                MinimalIds = miniEIDList.ToArray();
            }

            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}