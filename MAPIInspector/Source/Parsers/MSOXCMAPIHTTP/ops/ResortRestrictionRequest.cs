using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ResortRestrictionRequest structure.
    /// 2.2.5.15 ResortRestriction
    /// </summary>
    public class ResortRestrictionRequest : Block
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<bool> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public BlockT<bool> HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the MinimalIds field.
        /// </summary>
        public BlockT<uint> MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures that compose a restricted address book container.
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResortRestrictionRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            HasState = ParseAs<byte, bool>();
            if (HasState) State = Parse<STAT>();
            HasMinimalIds = ParseAs<byte, bool>();

            if (HasMinimalIds)
            {
                MinimalIdCount = ParseT<uint>();
                var miniEIDList = new List<MinimalEntryID>();
                for (int i = 0; i < MinimalIdCount; i++)
                {
                    miniEIDList.Add(Parse<MinimalEntryID>());
                }

                MinimalIds = miniEIDList.ToArray();
            }

            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "ResortRestrictionRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasMinimalIds, "HasMinimalIds");
            AddChildBlockT(MinimalIdCount, "MinimalIdCount");
            AddLabeledChildren(MinimalIds, "MinimalIds");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}