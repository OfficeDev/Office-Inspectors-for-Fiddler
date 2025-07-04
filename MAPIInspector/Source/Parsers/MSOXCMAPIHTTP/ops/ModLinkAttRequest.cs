using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ModLinkAttRequest structure.
    /// 2.2.5.10 ModLinkAtt
    /// </summary>
    public class ModLinkAttRequest : Block
    {
        /// <summary>
        /// A set of bit flags that specify options to the server.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// A PropertyTag structure that specifies the property to be modified.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A MinimalEntryID structure that specifies the Minimal Entry ID of the address book row to be modified.
        /// </summary>
        public MinimalEntryID MinimalId;

        /// <summary>
        /// A Boolean value that specifies whether the EntryIdCount and EntryIds fields are present.
        /// </summary>
        public BlockT<bool> HasEntryIds;

        /// <summary>
        /// An unsigned integer that specifies the count of structures in the EntryIds field.
        /// </summary>
        public BlockT<uint> EntryIdCount;

        /// <summary>
        /// An array of entry IDs, each of which is either an EphemeralEntryID structure or a PermanentEntryID structure.
        /// </summary>
        public Block[] EntryIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModLinkAttRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            PropertyTag = Parse<PropertyTag>();
            MinimalId = Parse<MinimalEntryID>();
            HasEntryIds = ParseAs<byte, bool>();

            if (HasEntryIds)
            {
                EntryIdCount = ParseT<uint>();
                var tempObj = new List<Block>();

                for (int i = 0; i < EntryIdCount; i++)
                {
                    var currentByte = TestParse<byte>();
                    if (currentByte == 0x87)
                    {
                        tempObj.Add(Parse<EphemeralEntryID>());
                    }
                    else if (currentByte == 0x00)
                    {
                        tempObj.Add(Parse<PermanentEntryID>());
                    }
                }

                EntryIds = tempObj.ToArray();
            }

            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("ModLinkAttRequest");
            AddChildBlockT(Flags, "Flags");
            AddChild(PropertyTag, "PropertyTag");
            AddChild(MinimalId, "MinimalId");
            AddChildBlockT(HasEntryIds, "HasEntryIds");
            AddChildBlockT(EntryIdCount, "EntryIdCount");
            AddLabeledChildren(EntryIds, "EntryIds");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}