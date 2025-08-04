using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the DnToMinIdRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5 Request Types for Address Book Server Endpoint
    /// [MS-OXCMAPIHTTP] 2.2.5.4 DnToMinId
    /// </summary>
    public class DnToMinIdRequest : Block
    {
        /// <summary>
        /// The reserved field
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the NameCount and NameValues fields are present.
        /// </summary>
        public BlockT<bool> HasNames;

        /// <summary>
        /// An unsigned integer that specifies the number of null-terminated Unicode strings in the NameValues field.
        /// </summary>
        public BlockT<uint> NameCount;

        /// <summary>
        /// An array of null-terminated ASCII strings which are distinguished names (DNs) to be mapped to Minimal Entry IDs.
        /// </summary>
        public BlockString[] NameValues;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data returned from the server.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the DnToMinIdRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            HasNames = ParseAs<byte, bool>();
            if (HasNames)
            {
                NameCount = ParseT<uint>();
                var nameValues = new List<BlockString>();
                for (int i = 0; i < NameCount; i++)
                {
                    nameValues.Add(ParseStringA());
                }

                NameValues = nameValues.ToArray();
            }

            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "DnToMinIdRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(HasNames, "HasNames");
            AddChildBlockT(NameCount, "NameCount");
            AddLabeledChildren(NameValues, "NameValues");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
